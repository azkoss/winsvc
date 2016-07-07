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
    }
}
