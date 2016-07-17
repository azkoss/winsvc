using System;
using System.Threading;
using dummy_service;
using NUnit.Framework;
using winsvc.AccessMasks;
using winsvc.Structs;

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
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                service.Delete();
            }
        }

        [Test]
        public void StartService()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                service.Start(new string[] {});
                
                // TODO Wait until started
                Thread.Sleep(1000);

                SERVICE_STATUS status = new SERVICE_STATUS();
                service.Control(SERVICE_CONTROL.SERVICE_CONTROL_STOP, ref status);
            }
        }

        [Test]
        public void QueryServiceStatus()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                Assert.That(service.QueryServiceStatus().dwCurrentState, Is.EqualTo(SERVICE_STATE.SERVICE_STOPPED));

                service.Start(new string[]{});

                WaitForServiceToStart(service);

                Assert.That(service.QueryServiceStatus().dwCurrentState, Is.EqualTo(SERVICE_STATE.SERVICE_RUNNING));

                WaitForServiceToStop(service);
            }
        }

        private static void WaitForServiceToStop(IService service)
        {
            var status = new SERVICE_STATUS();
            service.Control(SERVICE_CONTROL.SERVICE_CONTROL_STOP, ref status);

            while (service.QueryServiceStatus().dwCurrentState != SERVICE_STATE.SERVICE_STOPPED)
            {
                Thread.Sleep(10);
            }
        }

        private static void WaitForServiceToStart(IService service)
        {
            while (service.QueryServiceStatus().dwCurrentState != SERVICE_STATE.SERVICE_RUNNING)
            {
                Thread.Sleep(10);
            }
        }

        [Test]
        public void QueryServiceConfig()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                var config = service.QueryServiceConfig();

                Assert.That(config.DisplayName, Is.EqualTo(DummyService.Name));

                // Service is cleaned up in TearDown
            }
        }
    }
}