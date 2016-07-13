using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using winsvc.AccessMasks;
using winsvc.Structs;

namespace winsvc
{
    internal sealed class Service : SafeHandleZeroOrMinusOneIsInvalid, IService
    {
        // ReSharper disable once InconsistentNaming
        private const int ERROR_INSUFFICIENT_BUFFER = 122;

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

        public void Control(SERVICE_CONTROL control, ref SERVICE_STATUS status)
        {
            if (!NativeMethods.ControlService(handle, control, ref status))
            {
                throw new Win32Exception();
            }
        }

        public QUERY_SERVICE_CONFIG QueryServiceConfig()
        {
            int needed = 0;
            if (NativeMethods.QueryServiceConfig(handle, IntPtr.Zero, 0, ref needed))
            {
                throw new ApplicationException("Unexpected success querying service config");
            }

            if (Marshal.GetLastWin32Error() != ERROR_INSUFFICIENT_BUFFER)
            {
                throw new Win32Exception();
            }

            var bufferPtr = Marshal.AllocHGlobal(needed);
            try
            {
                if (!NativeMethods.QueryServiceConfig(handle, bufferPtr, needed, ref needed))
                {
                    throw new Win32Exception();
                }

                return Marshal.PtrToStructure<QUERY_SERVICE_CONFIG>(bufferPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }
    }
}