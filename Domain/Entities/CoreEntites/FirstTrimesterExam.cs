using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class FirstTrimesterExam : BaseEntity<Guid>
    {
        public Guid PregnancyId { get; set; }
        public Pregnancy Pregnancy { get; set; } = default!;

        public DateTime Date { get; set; }
        public int Week { get; set; }

        public decimal CRL { get; set; } // Crown–Rump Length
        public decimal GSD { get; set; } // Gestational Sac Diameter
        public string? Notes { get; set; }
    }
}
