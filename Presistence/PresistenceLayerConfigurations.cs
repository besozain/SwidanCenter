using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presistence.unitOfWork;

namespace Presistence
{
    public static class PresistenceLayerConfigurations
    {
        public static IServiceCollection AddPresistenceConfig(this IServiceCollection Services, IConfiguration _configuration)
        {
            Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        _configuration.GetConnectionString("DefaultConnection"),
                          sqlOptions => sqlOptions.EnableRetryOnFailure()
                    ));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IDataSeeding, DataSeeding>();
            Services.AddScoped<INewDataSeeding, NewDataSeeding>();
            Services.AddScoped<IRawSqlExecutor, RawSqlExecutor>();
            return Services;
        }
    }
}
