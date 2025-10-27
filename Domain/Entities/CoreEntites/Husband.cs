using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class Husband : BaseEntity
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public bool? IsSmoking { get; set; }
        public bool? IsDiabetic { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
    }
}
