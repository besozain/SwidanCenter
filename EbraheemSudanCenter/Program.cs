
using System;
using Domain.Contracts;
using EbraheemSudanCenter.CustomMiddleWares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presistence;
using Presistence.Data;
using RideFix.CustomMiddlewares;
using Service;

namespace EbraheemSudanCenter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(opt =>
                     opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddServiceConfig();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Services Configurations
            builder.Services.AddPresistenceConfig(builder.Configuration); // Custom extension method to add persistence layer configurations
                                                                          // builder.Services.AddServiceConfig();// Custom extension method to add service layer configurations
            #endregion

            #region Invalid Model State Response Factory Configuration

            builder.Services.Configure<ApiBehaviorOptions>(ApiBehaviorOptions =>
            {
                ApiBehaviorOptions.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new RideFix.ErrorModels.ValidationError
                        {
                            Key = e.Key,
                            Errors = e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                        }).ToArray();
                    var Error = new RideFix.ErrorModels.ValidationErrorToReturn
                    {
                        Errors = errors,
                    };
                    return new BadRequestObjectResult(Error);
                };
            });

            #endregion

            #region CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularClient",
                    policy =>
                    {
                        policy
                            .WithOrigins("http://localhost:4200", "https://localhost:4200") // Angular
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<ApiResponseMiddleware>();

            #region Exception Handler Middleware Configuration
            app.UseMiddleware<CustomExceptionMiddleware>();
            #endregion

            #region Data Seeding Configuration
            using (var scope = app.Services.CreateScope())
            {
                var dataSeeding = scope.ServiceProvider.GetRequiredService<INewDataSeeding>();
                await dataSeeding.SeedAllAsync();
            }
            #endregion

            app.UseCors("AllowAngularClient");

            app.MapControllers();

            app.Run();
        }
    }
}
