using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Service.AutoMapperProfile;
using Service.CoreServices;
using ServiceAbstraction;
using ServiceAbstraction.CoreServices;

namespace Service
{
    public static class ServiceLayerConfigurations
    {
        public static IServiceCollection AddServiceConfig(this IServiceCollection Services)
        {
            Services.AddScoped<IServiceManager, ServiceManager>();

            Services.AddAutoMapper(typeof(PatientProfile));


            #region Core Services

            Services.AddScoped<IPatientService, PatientServices>();
            Services.AddScoped<Func<IPatientService>>(x => () => x.GetRequiredService<IPatientService>());

            Services.AddScoped<IStatsServices, StatsServices>();
            Services.AddScoped<Func<IStatsServices>>(x => () => x.GetRequiredService<IStatsServices>());

            #endregion
            return Services;
        }
    }
}
