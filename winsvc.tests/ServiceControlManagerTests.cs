using System;
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
            using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS_MASK.SC_MANAGER_CONNECT))
            using (scm.OpenService("Spooler", (uint) SERVICE_ACCESS_MASK.SERVICE_QUERY_STATUS))
            {

            }
        }
    }
}
