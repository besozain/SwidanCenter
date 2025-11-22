using EbraheemSudanCenter.GeneralResponses;

namespace EbraheemSudanCenter.CustomMiddleWares
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            // امسك الستريم الأصلي واستبدله مؤقتًا
            var originalBody = context.Response.Body;
            await using var buffer = new MemoryStream();
            context.Response.Body = buffer;

            await _next(context);

            // رجّع المؤشر للبداية واقرأ البودي كنص
            buffer.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(buffer).ReadToEndAsync();

            // رجّع الستريم الأصلي مكانه
            context.Response.Body = originalBody;

            // لو 2xx لفّ الرد، غير كده سيبه زي ما هو
            var status = context.Response.StatusCode;
            if (status >= 200 && status < 300)
            {
                // شيل أي Content-Length عشان هنغير البودي
                context.Response.Headers.ContentLength = null;

                object? payload = null;
                var contentType = context.Response.ContentType ?? string.Empty;

                // 1) لو فاضي: data = null
                if (string.IsNullOrWhiteSpace(bodyText))
                {
                    payload = null;
                }
                else if (IsJsonContentType(contentType) || LooksLikeJson(bodyText))
                {
                    // 2) لو شكله JSON: جرّب تفكّه
                    //    - لو already wrapped سيبه زي ما هو
                    if (IsAlreadyWrapped(bodyText))
                    {
                        // رجّع المحتوى كما هو بدون لف مزدوج
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(bodyText);
                        return;
                    }

                    try
                    {
                        payload = System.Text.Json.JsonSerializer.Deserialize<object>(bodyText);
                    }
                    catch
                    {
                        // 3) JSON مكسور: خليه string خام بدل ما نرمي استثناء
                        payload = bodyText;
                    }
                }
                else
                {
                    // 4) مش JSON (نص/HTML/CSV…): خليه string
                    payload = bodyText;
                }

                // ابنِ اللفّة الموحّدة
                var response = new ApiResponse<object>(
                    statusCode: status,
                    message: status == 204 ? "No content" : "Success",
                    data: payload
                );

                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
            }
            else
            {
                // غير 2xx: رجّع البودي الأصلي
                // (لو عايز توحّد الأخطاء برضه، انقل الكود فوق واعمله هنا)
                buffer.Seek(0, SeekOrigin.Begin);
                await buffer.CopyToAsync(originalBody);
            }
        }

        private static bool IsJsonContentType(string contentType)
            => contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase)
            || contentType.Contains("text/json", StringComparison.OrdinalIgnoreCase);

        private static bool LooksLikeJson(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (!char.IsWhiteSpace(c))
                    return c == '{' || c == '[';
            }
            return false;
        }

        // اعتبر الرد Already Wrapped لو بيحتوي مفاتيحنا القياسية
        private static bool IsAlreadyWrapped(string json)
        {
            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var root = doc.RootElement;
                return root.ValueKind == System.Text.Json.JsonValueKind.Object
                    && (root.TryGetProperty("statusCode", out _)
                        || root.TryGetProperty("status", out _))
                    && root.TryGetProperty("message", out _)
                    && root.TryGetProperty("data", out _);
            }
            catch
            {
                return false;
            }
        }
    }

}
