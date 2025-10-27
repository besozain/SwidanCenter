using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class OperationCategory : BaseEntity
    {
        public string Name { get; set; } = default!;
        public List<OperationType> OperationTypes { get; set; } = new();
    }
}
