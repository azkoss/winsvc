using System;
using System.Threading;
using NUnit.Framework;
using winsvc.AccessMasks;
using winsvc.Structs;

namespace winsvc.tests
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public void DeleteService()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS_MASK.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                service.Delete();
            }
        }

        [Test]
        public void StartService()
        {
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32)SCM_ACCESS_MASK.SC_MANAGER_CREATE_SERVICE))
            using (var service = ServiceControlManagerTests.CreateDummyService(scm))
            {
                service.Start(new string[] {});
                
                // TODO Wait until started
                Thread.Sleep(1000);

                SERVICE_STATUS status = new SERVICE_STATUS();
                service.Control(SERVICE_CONTROL.SERVICE_CONTROL_STOP, ref status);
                service.Delete();
            }
        }
    }
}