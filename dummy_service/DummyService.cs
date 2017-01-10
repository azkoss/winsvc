using System.ServiceProcess;

namespace frogmore.winsvc.dummy_service
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