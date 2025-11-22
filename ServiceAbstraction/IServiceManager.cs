using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceAbstraction.CoreServices;

namespace ServiceAbstraction
{  
    public interface IServiceManager
    {
        public IPatientService PatientService { get; }
        public IStatsServices statsService { get; }
    }
}
