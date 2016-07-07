using NUnit.Framework;

namespace winsvc.tests
{
    [TestFixture]
    public class ServiceControlManagerTests
    {
        [Test]
        public void OpenControlServiceManager()
        {
            using (new ServiceControlManager(null, 0))
            {
            }
        }

        [Test]
        public void OpenService()
        {
            using (var scm = new ServiceControlManager(null, 1))
            using (var service = scm.OpenService("Spooler", 4))
            {

            }
        }
    }
}
