using System.ServiceProcess;

namespace dummy_service
{
    public class DummyService : ServiceBase
    {
        public static string DisplayName = "Dummy Service";
        public static string SvcName = "DummyService";

        public DummyService()
        {
            ServiceName = SvcName;
            CanShutdown = true;
            CanPauseAndContinue = true;
            CanStop = true;
        }
    }
}