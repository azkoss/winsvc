using System;
using System.Linq;
using dummy_service;
using NUnit.Framework;
using winsvc.AccessMasks;

namespace winsvc.tests
{
    [TestFixture]
    public class ServiceControlManagerTests
    {
        [Test]
        public void OpenControlServiceManager()
        {
            // Just checking for a lack of exceptions at this stage
            using (ServiceControlManager.OpenServiceControlManager(null, 0))
            {
            }
        }

        [Test]
        public void OpenService()
        {
            // Again just checking for a lack of exceptions at this stage
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS_MASK.SC_MANAGER_CONNECT))
            using (scm.OpenService("Spooler", (UInt32) SERVICE_ACCESS_MASK.SERVICE_QUERY_STATUS))
            {

            }
        }

        [Test]
        public void CreateService()
        {

            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS_MASK.SC_MANAGER_CREATE_SERVICE))
            using (var service = CreateDummyService(scm))
            {
                service.Delete();
            }
        }

        [Test]
        public void EnumServicesStatus()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS_MASK.SC_MANAGER_ENUMERATE_SERVICE))
            {
                var services = scm.EnumServicesStatus();
                Assert.That(services.Count(s => s.ServiceName == "Spooler"), Is.EqualTo(1));
            }
        }

        public static IService CreateDummyService(IServiceControlManager scm)
        {
            var path = typeof(DummyService).Assembly.Location;

            return scm.CreateService(DummyService.Name, 
                DummyService.Name,
                (uint) SERVICE_ACCESS_MASK.SERVICE_ALL_ACCESS,
                (uint) SERVICE_TYPE.SERVICE_WIN32_OWN_PROCESS,
                (uint) SERVICE_START_TYPE.SERVICE_AUTO_START,
                (uint) SERVICE_ERROR_CONTROL.SERVICE_ERROR_NORMAL,
                path,
                "",
                IntPtr.Zero,
                "",
                null,
                null);
        }
    }
}
