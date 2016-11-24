using System.Linq;
using System.Threading;
using dummy_service;
using winsvc.Enumerations;
using winsvc.Flags;

namespace winsvc.tests
{
    public static class CleanUp
    {
        public static void DeleteDummyServiceIfItExists()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_ENUMERATE_SERVICE))
            {
                var services = scm.EnumServicesStatus();

                foreach (var serviceName in services.Select(serviceStatus => serviceStatus.ServiceName).Where(name => name.StartsWith(DummyService.Name) || name.StartsWith(DummyService.Name)))
                {
                    DeleteService(scm, serviceName);
                }
            }
        }

        private static void DeleteService(IServiceControlManager scm, string name)
        {
            using (var service = scm.OpenService(name, SERVICE_ACCESS.SERVICE_ALL_ACCESS))
            {
                if (service.QueryServiceStatus().dwCurrentState == SERVICE_STATE.SERVICE_RUNNING)
                {
                    service.Control(SERVICE_CONTROL.SERVICE_CONTROL_STOP);

                    while (service.QueryServiceStatus().dwCurrentState != SERVICE_STATE.SERVICE_STOPPED)
                    {
                        Thread.Sleep(10);
                    }
                }

                service.Delete();
            }
        }
    }
}