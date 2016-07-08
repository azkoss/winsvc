using System.Collections.Specialized;
using System.ServiceProcess;
using System.Threading;

namespace dummy_service
{
    public class DummyService : ServiceBase
    {
        private static readonly AutoResetEvent Event = new AutoResetEvent(false);

        public static string Name = "Dummy Service";

        private readonly Thread _thread = new Thread(() =>
        {
            Event.WaitOne();
        });

        public DummyService()
        {
            ServiceName = Name;
            CanShutdown = true;
            CanPauseAndContinue = false;
            CanStop = true;
        }

        protected override void OnStart(string[] args)
        {
            _thread.Start();
        }

        protected override void OnStop()
        {
            Event.Set();
        }
    }
}