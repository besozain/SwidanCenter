using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.CoreEntites;
using Service.Specification_Implementation.PatientsSpecifications;
using ServiceAbstraction.CoreServices;
using SharedData.DTOs;
using SharedData.DTOs.PatientsDtos;
using SharedData.QueryModels;

namespace Service.CoreServices
{
    public class PatientServices(IUnitOfWork unitOfWork, IRawSqlExecutor rawSqlExecutor , IMapper mapper) : IPatientService
    {
        public async Task<IEnumerable<PatientDTO>> GetPatientsAsync(PatientQueryData patientQueryData)
        {
            var Repo = unitOfWork.GetRepository<Patient, Guid>();
            var spec = new SearchPatientsSpecification(patientQueryData);
            var patients = await Repo.GetAllWithSpecAsync(spec);
            return mapper.Map<IEnumerable<PatientDTO>>(patients);
        }
    }
}
