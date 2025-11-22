using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using ServiceAbstraction.CoreServices;
using SharedData.DTOs.StatsDtos;

namespace Service.CoreServices
{
    public class StatsServices(IUnitOfWork unitOfWork, IRawSqlExecutor rawSqlExecutor) : IStatsServices
    {
        public async Task<HomeBasicStatisticsDTO> GetDashboardStatisticsAsync()
        {
            var today = DateTime.Today;
            var dto = await rawSqlExecutor.QuerySingleAsync<HomeBasicStatisticsDTO>(
                "EXEC GetDashboardStats @Today = {0}", today
            );
            return dto;
        }
    }
}
