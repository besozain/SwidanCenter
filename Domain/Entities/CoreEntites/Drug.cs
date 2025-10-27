using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class Drug : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        // في Patient
        public List<PatientMedicationCourse> PatientMedicationCourses { get; set; } = new();

    }
}
