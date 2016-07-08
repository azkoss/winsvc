using System.ServiceProcess;

namespace dummy_service
{
    public class Program
    {
        private static void Main()
        {
            ServiceBase.Run(new DummyService());
        }
    }
}
