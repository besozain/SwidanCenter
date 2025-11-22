using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.DTOs.StatsDtos
{
    public class HomeBasicStatisticsDTO
    {
        public int CurrentDayPatients { get; set; }
        public int CurrentDayOperations { get; set; }
        public int NewPatientsToday { get; set; }
        public int TotalPatients { get; set; }
    }
}
