using System;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;
using winsvc.AccessMasks;

namespace winsvc
{
    public interface IService : IDisposable
    {
        void Delete();
        void Start(string[] serviceArgVectors);
        void Control(SERVICE_CONTROL control, ref ServiceStatus status);
    }

    internal sealed class Service : SafeHandleZeroOrMinusOneIsInvalid, IService
    {
        public Service(IntPtr serviceHandle) : base(true)
        {
            handle = serviceHandle;
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseServiceHandle(handle);
        }

        public void Delete()
        {
            if (!NativeMethods.DeleteService(handle))
            {
                throw new Win32Exception();
            }
        }

        public void Start(string[] serviceArgVectors)
        {
            if (!NativeMethods.StartService(handle, serviceArgVectors.Length, serviceArgVectors))
            {
                throw new Win32Exception();
            }
        }

        public void Control(SERVICE_CONTROL control, ref ServiceStatus status)
        {
            if (!NativeMethods.ControlService(handle, control, ref status))
            {
                throw new Win32Exception();
            }
        }
    }
}