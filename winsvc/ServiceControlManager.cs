using System;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;

namespace winsvc
{
    public sealed class ServiceControlManager : SafeHandleZeroOrMinusOneIsInvalid
    {
        public ServiceControlManager(string machineName, UInt32 desiredAccess) : base(true)
        {
            handle = NativeMethods.OpenSCManager(machineName, null, desiredAccess);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseServiceHandle(handle);
        }
    }
}