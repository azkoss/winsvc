using System;
using Microsoft.Win32.SafeHandles;

namespace winsvc
{
    public interface IService : IDisposable
    {
        
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
    }
}