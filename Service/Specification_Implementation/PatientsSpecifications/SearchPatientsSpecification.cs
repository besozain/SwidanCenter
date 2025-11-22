using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.CoreEntites;
using SharedData.QueryModels;

namespace Service.Specification_Implementation.PatientsSpecifications
{
    internal class SearchPatientsSpecification : Specification<Patient, Guid>
    {
        public SearchPatientsSpecification(PatientQueryData patientQueryData)
            : base(p => (string.IsNullOrWhiteSpace(patientQueryData.NameorPhoneNumber) ||
            (p.FullName.ToLower().Contains(patientQueryData.NameorPhoneNumber.ToLower())
            || p.PhoneNumber.ToLower().Contains(patientQueryData.NameorPhoneNumber.ToLower())
            )))
        {

            if (patientQueryData.Take.HasValue && patientQueryData.Take.Value > 0)
            {
                ApplyPaging(patientQueryData.PageIndex.GetValueOrDefault(defaultValue: 0)
                    * patientQueryData.Take.Value, patientQueryData.Take.Value);
            }
        }
    }
}

