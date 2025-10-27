using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class Operation : BaseEntity
    {
        public List<Patient> Patients { get; set; } = default!;

        [ForeignKey("OperationType")]
        public int OperationTypeId { get; set; }
        public OperationType OperationType { get; set; } = default!;
        public int OperationCategoryId { get; set; }
        public OperationCategory OperationCategory { get; set; } = default!;
        public List<PatientOperations> PatientOperations { get; set; } = new();

    }
}
