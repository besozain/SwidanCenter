using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class TestCategory : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public List<TestType> TestTypes { get; set; } = new();
    }
}
