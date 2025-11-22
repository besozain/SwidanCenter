using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using SharedData.DTOs;
using SharedData.DTOs.PatientsDtos;
using SharedData.QueryModels;

namespace Presentation.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PatientController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("GetPatients")]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients([FromQuery] PatientQueryData patientQueryData)
        {
            var patients = await serviceManager.PatientService.GetPatientsAsync(patientQueryData);
            return Ok(patients);
        }

        [HttpGet("PatientsCount")]
        public async Task<ActionResult<int>> GetPatientsCount()
        {
            var Count = await serviceManager.PatientService.GetPatientsCountAsync();
            return Ok(Count);
        }
    }

}
