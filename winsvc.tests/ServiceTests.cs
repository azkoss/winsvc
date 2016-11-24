using System;
using System.Linq;
using dummy_service;
using NUnit.Framework;
using winsvc.Enumerations;
using winsvc.Flags;

namespace winsvc.tests
{
    [TestFixture]
    public class ServiceTests
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
        public void DeleteService()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                service.Delete();
            }
        }

        [Test]
        public void StartService()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                service.Start(new string[] {});
                service.WaitForServiceToStart();

                service.StopServiceAndWait();
            }
        }

        [Test]
        public void QueryServiceStatus()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                Assert.That(service.QueryServiceStatus().dwCurrentState, Is.EqualTo(SERVICE_STATE.SERVICE_STOPPED));

                service.Start(new string[]{});

                service.WaitForServiceToStart();

                Assert.That(service.QueryServiceStatus().dwCurrentState, Is.EqualTo(SERVICE_STATE.SERVICE_RUNNING));

                service.StopServiceAndWait();
            }
        }

        [Test]
        public void QueryServiceConfig()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                var config = service.QueryServiceConfig();

                Assert.That(config.DisplayName, Is.EqualTo(DummyService.Name));

                // Service is cleaned up in TearDown
            }
        }

        [Test]
        public void QueryServiceStatusEx()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                var status = service.QueryServiceStatusEx();
                Assert.That(status.currentState, Is.EqualTo(SERVICE_STATE.SERVICE_STOPPED));

                service.Start(new string[] {});
                service.WaitForServiceToStart();
                Assert.That(service.QueryServiceStatusEx().currentState, Is.EqualTo(SERVICE_STATE.SERVICE_RUNNING));

                service.StopServiceAndWait();
            }
        }

        [Test]
        public void Description()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            {
                using (var service = ServiceControlManagerTests.CreateDummyService(scm))
                {
                    service.Description = "Service Description";
                }
                using (var service = scm.OpenService(DummyService.Name, SERVICE_ACCESS.SERVICE_QUERY_CONFIG))
                {
                    Assert.That(service.Description, Is.EqualTo("Service Description"));
                }
            }
        }

        [Test]
        public void ChangeServiceConfig()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                // No changes should not throw
                service.ChangeServiceConfig(
                    SERVICE_TYPE.SERVICE_NO_CHANGE, 
                    SERVICE_START_TYPE.SERVICE_NO_CHANGE,
                    SERVICE_ERROR_CONTROL.SERVICE_NO_CHANGE,
                    null, 
                    null,
                    IntPtr.Zero, 
                    null, 
                    null,
                    null,
                    null);

                // Set service type to share process
                service.ChangeServiceConfig(
                    SERVICE_TYPE.SERVICE_WIN32_SHARE_PROCESS, 
                    SERVICE_START_TYPE.SERVICE_NO_CHANGE,
                    SERVICE_ERROR_CONTROL.SERVICE_NO_CHANGE,
                    null, 
                    null,
                    IntPtr.Zero, 
                    null, 
                    null,
                    null,
                    null);
                Assert.That(service.QueryServiceConfig().ServiceType, Is.EqualTo((uint) SERVICE_TYPE.SERVICE_WIN32_SHARE_PROCESS));
                
                // Set start type to disabled
                service.ChangeServiceConfig(
                    SERVICE_TYPE.SERVICE_NO_CHANGE, 
                    SERVICE_START_TYPE.SERVICE_DISABLED,
                    SERVICE_ERROR_CONTROL.SERVICE_NO_CHANGE,
                    null, 
                    null,
                    IntPtr.Zero, 
                    null, 
                    null,
                    null,
                    null);
                Assert.That(service.QueryServiceConfig().StartType, Is.EqualTo((uint) SERVICE_START_TYPE.SERVICE_DISABLED));

                // Set error control to critical
                service.ChangeServiceConfig(
                    SERVICE_TYPE.SERVICE_NO_CHANGE,
                    SERVICE_START_TYPE.SERVICE_NO_CHANGE,
                    SERVICE_ERROR_CONTROL.SERVICE_ERROR_CRITICAL,
                    null,
                    null,
                    IntPtr.Zero,
                    null,
                    null,
                    null,
                    null);
                Assert.That(service.QueryServiceConfig().ErrorControl,
                    Is.EqualTo((uint) SERVICE_ERROR_CONTROL.SERVICE_ERROR_CRITICAL));

                service.ChangeServiceConfig(
                    SERVICE_TYPE.SERVICE_NO_CHANGE,
                    SERVICE_START_TYPE.SERVICE_NO_CHANGE,
                    SERVICE_ERROR_CONTROL.SERVICE_NO_CHANGE,
                    null,
                    null,
                    IntPtr.Zero,
                    null,
                    null,
                    null,
                    "New Display Name");
                Assert.That(service.QueryServiceConfig().DisplayName, Is.EqualTo("New Display Name"));
            }
        }

        [Test]
        public void EnumDependentServices()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_CREATE_SERVICE | SCM_ACCESS.SC_MANAGER_ENUMERATE_SERVICE))
            {
                // Just pick the first two services to be dependencies
                var dependentServiceNames = scm.EnumServicesStatus().Select(ss => ss.ServiceName).Take(1).ToList();

                var path = typeof(DummyService).Assembly.Location;

                using (scm.CreateService(
                    DummyService.Name,
                    DummyService.Name,
                    SERVICE_ACCESS.SERVICE_ALL_ACCESS,
                    SERVICE_TYPE.SERVICE_WIN32_OWN_PROCESS,
                    SERVICE_START_TYPE.SERVICE_AUTO_START,
                    SERVICE_ERROR_CONTROL.SERVICE_ERROR_NORMAL,
                    path,
                    "",
                    IntPtr.Zero,
                    dependentServiceNames,
                    null,
                    null))
                {
                }

                using (var service = scm.OpenService(dependentServiceNames.First(), SERVICE_ACCESS.SERVICE_ENUMERATE_DEPENDENTS))
                {
                    var serviceName = service.EnumDependentServices(SERVICE_STATES.SERVICE_STATE_ALL).Select(ss => ss.ServiceName).First();

                    Assert.That(serviceName, Is.EqualTo(DummyService.Name));
                }

            }
        }


    }
}