using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedData.DTOs.StatsDtos;

namespace ServiceAbstraction.CoreServices
{
    public interface IStatsServices
    {
        public Task<HomeBasicStatisticsDTO> GetDashboardStatisticsAsync();

    }
}
