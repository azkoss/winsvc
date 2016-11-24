using System.Threading;
using winsvc.Enumerations;

namespace winsvc.tests
{
    internal static class ServiceMethodExtensions
    {
        public static void StopServiceAndWait(this IService service)
        {
            StopService(service);
            WaitForServiceToStop(service);
        }

        public static void WaitForServiceToStart(this IService service)
        {
            while (service.QueryServiceStatus().dwCurrentState != SERVICE_STATE.SERVICE_RUNNING)
            {
                Thread.Sleep(10);
            }
        }

        private static void StopService(this IService service)
        {
            service.Control(SERVICE_CONTROL.SERVICE_CONTROL_STOP);
        }

        private static void WaitForServiceToStop(this IService service)
        {
            while (service.QueryServiceStatus().dwCurrentState != SERVICE_STATE.SERVICE_STOPPED)
            {
                Thread.Sleep(10);
            }
        }
    }
}