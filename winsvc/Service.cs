using System;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;

namespace winsvc
{
    public interface IService : IDisposable
    {
        void Delete();
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
    }
}