using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class PatientOperations : BaseEntity
    {
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = default!;
        public int OperationId { get; set; }
        public Operation Operation { get; set; } = default!;

    }
}
