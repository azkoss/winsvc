using System;
using System.ComponentModel;
using System.Linq;
using dummy_service;
using NUnit.Framework;
using winsvc.AccessMasks;

namespace winsvc.tests
{
    [TestFixture]
    public class ServiceControlManagerTests
    {
        [SetUp]
        public void Setup()
        {
            CleanUp.DeleteDummyServiceIfItExists();
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp.DeleteDummyServiceIfItExists();
        }

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
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS.SC_MANAGER_CONNECT))
            using (scm.OpenService("Spooler", (UInt32) SERVICE_ACCESS.SERVICE_QUERY_STATUS))
            {
                // Service is cleaned up in TearDown
            }
        }

        [Test]
        public void OpenServiceFailure()
        {
            // Check we get an exception if we try and open a service that doesn't exist

            // ReSharper disable once InconsistentNaming
            const int ERROR_SERVICE_DOES_NOT_EXIST = 1060;

            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS.SC_MANAGER_CONNECT))
            {
                // ReSharper disable once AccessToDisposedClosure
                Assert.That(() => scm.OpenService("Non existant service name", (UInt32) SERVICE_ACCESS.SERVICE_QUERY_STATUS), 
                    Throws.TypeOf<Win32Exception>()
                          .With.Property("NativeErrorCode").EqualTo(ERROR_SERVICE_DOES_NOT_EXIST));
            }
        }

        [Test]
        public void CreateService()
        {

            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (CreateDummyService(scm))
            {
                // Service is cleaned up in TearDown
            }
        }

        [Test]
        public void EnumServicesStatus()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS.SC_MANAGER_ENUMERATE_SERVICE))
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
                (uint) SERVICE_ACCESS.SERVICE_ALL_ACCESS,
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
