using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using SharedData.DTOs.StatsDtos;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("DashboardStats")]
        public async Task<ActionResult<HomeBasicStatisticsDTO>> GetDashboardStats()
        {
            var stats = await serviceManager.statsService.GetDashboardStatisticsAsync();
            return Ok(stats);
        }
    }
}
