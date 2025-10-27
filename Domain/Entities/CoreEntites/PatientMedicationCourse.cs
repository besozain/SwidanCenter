using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class PatientMedicationCourse : BaseEntity
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public int DrugId { get; set; }
        public Drug Drug { get; set; } = default!;
    }
}
