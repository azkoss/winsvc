using System;
using System.Linq;
using dummy_service;
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
                if (services.Any(s => s.DisplayName.StartsWith(DummyService.Name) || s.ServiceName.StartsWith(DummyService.Name)))
                {
                    DeleteService(scm);
                }
            }
        }

        private static void DeleteService(IServiceControlManager scm)
        {
            using (var service = scm.OpenService(DummyService.Name, SERVICE_ACCESS.SERVICE_ALL_ACCESS))
            {
                service.Delete();
            }
        }
    }
}