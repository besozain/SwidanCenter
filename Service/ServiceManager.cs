using ServiceAbstraction;
using ServiceAbstraction.CoreServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager(Func<IPatientService> PatientFactory, Func<IStatsServices> StatsFactory) : IServiceManager
    {
        public IPatientService PatientService => PatientFactory.Invoke();

        public IStatsServices statsService => StatsFactory.Invoke();
    }
}
