using System.Linq;
using frogmore.winsvc.dummy_service;
using frogmore.winsvc.Enumerations;
using frogmore.winsvc.Flags;

namespace frogmore.winsvc.tests
{
    public static class CleanUp
    {
        public static void DeleteDummyServiceIfItExists()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_ENUMERATE_SERVICE))
            {
                var services = scm.EnumServicesStatus(SERVICE_TYPE.SERVICE_WIN32, SERVICE_STATE_FLAGS.SERVICE_STATE_ALL);

                foreach (var serviceName in services.Select(serviceStatus => serviceStatus.ServiceName).Where(name => name.StartsWith(DummyService.DisplayName) || name.StartsWith(DummyService.SvcName)))
                {
                    DeleteService(scm, serviceName);
                }
            }
        }

        private static void DeleteService(IServiceControlManager scm, string name)
        {
            using (var service = scm.OpenService(name, SERVICE_ACCESS.SERVICE_ALL_ACCESS))
            {
                if (service.QueryStatus().dwCurrentState == SERVICE_STATE.SERVICE_RUNNING)
                {
                    service.StopServiceAndWait();
                }

                service.Delete();
            }
        }
    }
}