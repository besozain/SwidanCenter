using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CoreEntites
{
    public class SecondThirdTrimesterExam : BaseEntity
    {
        public int PregnancyId { get; set; }
        public Pregnancy Pregnancy { get; set; } = default!;

        public DateTime Date { get; set; }
        public int Week { get; set; }

        public decimal FL { get; set; } // Femur Length
        public decimal AC { get; set; } // Abdominal Circumference
        public decimal BPD { get; set; } // Biparietal Diameter
        public string Notes { get; set; }
    }
}
