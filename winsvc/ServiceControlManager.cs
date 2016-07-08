using System;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;

namespace winsvc
{
    public sealed class ServiceControlManager : SafeHandleZeroOrMinusOneIsInvalid, IServiceControlManager
    {
        private ServiceControlManager(string machineName, UInt32 desiredAccess) : base(true)
        {
            handle = NativeMethods.OpenSCManager(machineName, null, desiredAccess);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }

        public static IServiceControlManager OpenServiceControlManager(string machineName, UInt32 desiredAccess)
        {
            return new ServiceControlManager(machineName, desiredAccess);
        }

        public IService OpenService(string serviceName, UInt32 desiredAccess)
        {
            var serviceHandle = NativeMethods.OpenService(handle, serviceName, desiredAccess);
            if (serviceHandle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return new Service(serviceHandle);
        }

        public IService CreateService(string serviceName, string displayName, uint desiredAccess, uint serviceType, uint startType,
                                        uint errorControl, string binaryPathName, string loadOrderGroup, IntPtr tagId, string dependencies,
                                        string serviceStartName, string password)
        {
            var serviceHandle = NativeMethods.CreateService(handle, serviceName, displayName, desiredAccess, serviceType, startType, errorControl, binaryPathName, loadOrderGroup, tagId, dependencies, serviceStartName, password);
            if (serviceHandle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return new Service(serviceHandle);
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseServiceHandle(handle);
        }
    }
}