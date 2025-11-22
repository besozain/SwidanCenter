using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.CoreEntites;
using SharedData.DTOs;
using SharedData.DTOs.PatientsDtos;
using SharedData.QueryModels;

namespace ServiceAbstraction.CoreServices
{
    public interface IPatientService
    {
        public Task<IEnumerable<PatientDTO>> GetPatientsAsync(PatientQueryData patientQueryData);
        public Task<int> GetPatientsCountAsync();
    }
}
