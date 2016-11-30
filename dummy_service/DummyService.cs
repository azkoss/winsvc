using System.ServiceProcess;
using System.Threading;

namespace dummy_service
{
    public class DummyService : ServiceBase
    {
        private readonly AutoResetEvent _event = new AutoResetEvent(false);

        public static string DisplayName = "Dummy Service";
        public static string SvcName = "DummyService";
        private Thread _thread;

        public DummyService()
        {
            ServiceName = SvcName;
            CanShutdown = true;
            CanPauseAndContinue = true;
            CanStop = true;
        }

        protected override void OnStart(string[] args)
        {
            _thread = new Thread(() =>
            {
                _event.WaitOne();

            });
            _thread.Start();
        }

        protected override void OnStop()
        {
            _event.Set();
            _thread.Join();
        }
    }
}