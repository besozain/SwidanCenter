using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.CoreEntites;
using SharedData.DTOs.PatientsDtos;

namespace Service.AutoMapperProfile
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<Patient,PatientDTO>().ReverseMap();
            CreateMap<IEnumerable<Patient>, IEnumerable<PatientDTO>>().ReverseMap();
        }
    }
}
