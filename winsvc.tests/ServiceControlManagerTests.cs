using System;
using System.ComponentModel;
using System.Linq;
using frogmore.winsvc.dummy_service;
using frogmore.winsvc.Enumerations;
using frogmore.winsvc.Flags;
using NUnit.Framework;

namespace frogmore.winsvc.tests
{
    [TestFixture]
    public class ServiceControlManagerTests
    {
        // ReSharper disable once InconsistentNaming
        private const int ERROR_ACCESS_DENIED = 5;

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
        public void OpenControlServiceManagerFailure()
        {
            // ReSharper disable once InconsistentNaming
            const int RPC_S_SERVER_UNAVAILABLE = 1722;

            // Opening the service control manager on a server with an incorrect name (with spaces) throws quickly
            Assert.That(() => ServiceControlManager.OpenServiceControlManager("aa aa", 0), 
                Throws.TypeOf<Win32Exception>()
                .With.Property("NativeErrorCode").EqualTo(RPC_S_SERVER_UNAVAILABLE ));
        }

        [Test]
        public void OpenService()
        {
            // Again just checking for a lack of exceptions at this stage
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CONNECT | SCM_ACCESS.SC_MANAGER_ENUMERATE_SERVICE))
            {
                var serviceName = scm.EnumServicesStatus(SERVICE_TYPE.SERVICE_WIN32, SERVICE_STATE_FLAGS.SERVICE_STATE_ALL).Select(ss => ss.ServiceName).First();
                using (scm.OpenService(serviceName, SERVICE_ACCESS.SERVICE_QUERY_STATUS))
                {
                    // Service is cleaned up in TearDown
                }
            }
        }

        [Test]
        public void OpenServiceFailure()
        {
            // Check we get an exception if we try and open a service that doesn't exist

            // ReSharper disable once InconsistentNaming
            const int ERROR_SERVICE_DOES_NOT_EXIST = 1060;

            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CONNECT))
            {
                // ReSharper disable once AccessToDisposedClosure
                Assert.That(() => scm.OpenService("Non existant service name", SERVICE_ACCESS.SERVICE_QUERY_STATUS), 
                    Throws.TypeOf<Win32Exception>()
                          .With.Property("NativeErrorCode").EqualTo(ERROR_SERVICE_DOES_NOT_EXIST));
            }
        }

        [Test]
        public void CreateService()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (CreateDummyService(scm))
            {
                // Service is cleaned up in TearDown
            }
        }

        [Test]
        public void CreateServiceFailure()
        {
            // Create should CreateServiceFailure() WithOperator insufficient permissions
            var scm = ServiceControlManager.OpenServiceControlManager(null, 0);
            Assert.That(() => CreateDummyService(scm), Throws.TypeOf<Win32Exception>().With.Property("NativeErrorCode").EqualTo(ERROR_ACCESS_DENIED));
        }

        [Test]
        public void EnumServicesStatus()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE | SCM_ACCESS.SC_MANAGER_ENUMERATE_SERVICE))
            using (CreateDummyService(scm))
            {
                var services = scm.EnumServicesStatus(SERVICE_TYPE.SERVICE_WIN32, SERVICE_STATE_FLAGS.SERVICE_STATE_ALL);
                Assert.That(services.Count(s => s.ServiceName == DummyService.SvcName), Is.EqualTo(1));
            }
        }

        [Test]
        public void EnumServicesStatusFailure()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, 0))
            {
                // ReSharper disable once AccessToDisposedClosure
                Assert.That(() => scm.EnumServicesStatus(SERVICE_TYPE.SERVICE_WIN32, SERVICE_STATE_FLAGS.SERVICE_STATE_ALL).ToList(), 
                    Throws.TypeOf<Win32Exception>()
                          .With.Property("NativeErrorCode").EqualTo(ERROR_ACCESS_DENIED));
            }
        }

        [Test]
        public void EnumServicesStatusEx()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE | SCM_ACCESS.SC_MANAGER_ENUMERATE_SERVICE))
            using (CreateDummyService(scm))
            {
                var service = scm.EnumServicesStatusEx(SERVICE_TYPE.SERVICE_WIN32, SERVICE_STATE_FLAGS.SERVICE_STATE_ALL).First(s => s.ServiceName == DummyService.SvcName);
                Assert.That(service.ServiceStatusProcess.currentState, Is.EqualTo(SERVICE_STATE.SERVICE_STOPPED));
            }
        }

        [Test]
        public void GetServiceKeyName()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            {
                using (CreateDummyService(scm))
                {
                }
                Assert.That(scm.GetServiceKeyName(DummyService.DisplayName), Is.EqualTo(DummyService.SvcName));
            }
        }

        [Test]
        public void GetServiceDisplayName()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            {
                using (CreateDummyService(scm))
                {
                }
                Assert.That(scm.GetServiceDisplayName(DummyService.SvcName), Is.EqualTo(DummyService.DisplayName));
            }
        }

        public static IService CreateDummyService(IServiceControlManager scm)
        {
            var path = typeof(DummyService).Assembly.Location;

            return scm.CreateService(DummyService.SvcName, 
                DummyService.DisplayName,
                SERVICE_ACCESS.SERVICE_ALL_ACCESS,
                SERVICE_TYPE.SERVICE_WIN32_OWN_PROCESS,
                SERVICE_START_TYPE.SERVICE_AUTO_START,
                SERVICE_ERROR_CONTROL.SERVICE_ERROR_NORMAL,
                path,
                "",
                IntPtr.Zero,
                null,
                null,
                null);
        }
    }
}
