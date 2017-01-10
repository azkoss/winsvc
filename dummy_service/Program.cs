using System.ServiceProcess;

namespace frogmore.winsvc.dummy_service
{
    internal static class Program
    {
        private static void Main()
        {
            ServiceBase.Run(new DummyService());
        }
    }
}
