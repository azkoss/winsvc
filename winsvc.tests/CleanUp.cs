using System;
using System.Linq;
using dummy_service;
using winsvc.AccessMasks;

namespace winsvc.tests
{
    public static class CleanUp
    {
        public static void DeleteDummyServiceIfItExists()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS_MASK.SC_MANAGER_ENUMERATE_SERVICE))
            {
                var services = scm.EnumServicesStatus();
                if (services.Any(s => s.ServiceName == DummyService.Name))
                {
                    DeleteService(scm);
                }
            }
        }

        private static void DeleteService(IServiceControlManager scm)
        {
            using (var service = scm.OpenService(DummyService.Name, (uint) SERVICE_ACCESS_MASK.SERVICE_ALL_ACCESS))
            {
                service.Delete();
            }
        }
    }
}