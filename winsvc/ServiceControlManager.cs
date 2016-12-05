using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using winsvc.Enumerations;
using winsvc.Flags;
using winsvc.Structs;

namespace winsvc
{
    public sealed class ServiceControlManager : SafeHandleZeroOrMinusOneIsInvalid, IServiceControlManager
    {
        // ReSharper disable InconsistentNaming
        private const int ERROR_MORE_DATA = 234;
        private const int ERROR_INSUFFICIENT_BUFFER = 122;
        private const int SC_ENUM_PROCESS_INFO = 0;
        // ReSharper restore InconsistentNaming

        private ServiceControlManager(string machineName, SCM_ACCESS desiredAccess) : base(true)
        {
            handle = NativeMethods.OpenSCManager(machineName, null, (uint) desiredAccess);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Establishes a connection to the service control manager on the specified computer and opens the specified
        /// service control manager database.
        /// 
        /// The returned ServiceControlManager implements IDisposable and should be used with a using statement.
        /// </summary>
        /// <param name="machineName">
        /// The name of the target computer. If the pointer is NULL or points to an empty string, the function connects 
        /// to the service control manager on the local computer.</param>
        /// <param name="desiredAccess">
        /// The access to the service control manager.</param>
        /// <returns>An instance of IServiceControlManager</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        public static IServiceControlManager OpenServiceControlManager(string machineName, SCM_ACCESS desiredAccess)
        {
            return new ServiceControlManager(machineName, desiredAccess);
        }

        public IService OpenService(string serviceName, SERVICE_ACCESS desiredAccess)
        {
            var serviceHandle = NativeMethods.OpenService(handle, serviceName, (uint) desiredAccess);
            if (serviceHandle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return new Service(serviceHandle);
        }

        public IService CreateService(
            string serviceName, 
            string displayName, 
            SERVICE_ACCESS desiredAccess, 
            SERVICE_TYPE serviceType,
            SERVICE_START_TYPE startType,
            SERVICE_ERROR_CONTROL errorControl, 
            string binaryPathName, 
            string loadOrderGroup, 
            IntPtr tagId, 
            ICollection<string> dependencies,
            string serviceStartName, 
            string password)
        {
            string deps = (dependencies?.Any() ?? false) ? string.Join("\0", dependencies) : null;

            var serviceHandle = NativeMethods.CreateService(
                    handle, 
                    serviceName, 
                    displayName, 
                    (uint) desiredAccess, 
                    (uint) serviceType,
                    (uint) startType, 
                    (uint) errorControl, 
                    binaryPathName, 
                    loadOrderGroup, 
                    tagId, 
                    deps, 
                    serviceStartName, 
                    password
            );

            if (serviceHandle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return new Service(serviceHandle);
        }

        public IEnumerable<ENUM_SERVICE_STATUS> EnumServicesStatus(SERVICE_TYPE serviceType, SERVICE_STATE_FLAGS serviceState)
        {
            int needed = 0;
            int servicesReturned = 0;
            uint resumeHandle = 0;

            if (NativeMethods.EnumServicesStatus(handle, serviceType, serviceState, IntPtr.Zero, 0, ref needed, ref servicesReturned, ref resumeHandle))
            {
                throw new ApplicationException("Unexpected success enumerating services with zero buffer");
            }

            // We expect an ERROR_MORE_DATA error as the buffer size passed in was zero, otherwise something strage is going on
            if (Marshal.GetLastWin32Error() != ERROR_MORE_DATA) 
            {
                throw new Win32Exception();
            }

            IntPtr bufferPtr = Marshal.AllocHGlobal(needed);
            var ptr = bufferPtr;
            try
            {
                if (!NativeMethods.EnumServicesStatus(handle, serviceType, serviceState, bufferPtr, needed, ref needed, ref servicesReturned, ref resumeHandle))
                {
                    throw new Win32Exception();
                }

                for (int i = 0; i < servicesReturned; i++)
                {
                    yield return (ENUM_SERVICE_STATUS) Marshal.PtrToStructure(ptr, typeof(ENUM_SERVICE_STATUS));
                    ptr += Marshal.SizeOf(typeof(ENUM_SERVICE_STATUS));
                }
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }

        public IEnumerable<ENUM_SERVICE_STATUS_PROCESS> EnumServicesStatusEx(SERVICE_TYPE serviceType, SERVICE_STATE_FLAGS serviceStateFlags)
        {
            int needed = 0;
            int servicesReturned = 0;
            uint resumeHandle = 0;

            if (NativeMethods.EnumServicesStatusEx(
                handle, 
                SC_ENUM_PROCESS_INFO, 
                serviceType, 
                serviceStateFlags, 
                IntPtr.Zero, 
                0, 
                ref needed, 
                ref servicesReturned, 
                ref resumeHandle, 
                null))
            {
                throw new ApplicationException("Unexpected success enumerating services with zero buffer");
            }

            // We expect an ERROR_MORE_DATA error as the buffer size passed in was zero, otherwise something strage is going on
            if (Marshal.GetLastWin32Error() != ERROR_MORE_DATA)
            {
                throw new Win32Exception();
            }

            IntPtr bufferPtr = Marshal.AllocHGlobal(needed);
            var ptr = bufferPtr;
            try
            {
                if (!NativeMethods.EnumServicesStatusEx(
                    handle,
                    SC_ENUM_PROCESS_INFO,
                    serviceType, 
                    serviceStateFlags, 
                    bufferPtr,
                    needed,
                    ref needed,
                    ref servicesReturned,
                    ref resumeHandle,
                    null))
                {
                    throw new Win32Exception();
                }

                for (int i = 0; i < servicesReturned; i++)
                {
                    yield return (ENUM_SERVICE_STATUS_PROCESS)Marshal.PtrToStructure(ptr, typeof(ENUM_SERVICE_STATUS_PROCESS));
                    ptr += Marshal.SizeOf(typeof(ENUM_SERVICE_STATUS_PROCESS));
                }
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }

        public string GetServiceKeyName(string displayName)
        {
            Int32 needed = 0;

            if (NativeMethods.GetServiceKeyName(handle, displayName, IntPtr.Zero, ref needed))
            {
                throw new ApplicationException($"Unexpected success in {nameof(ServiceControlManager)}.{nameof(GetServiceKeyName)}");
            }

            if (Marshal.GetLastWin32Error() != ERROR_INSUFFICIENT_BUFFER)
            {
                throw new Win32Exception();
            }

            needed = 2 * (needed + 1); // For null terminator and wide chars

            IntPtr bufferPtr = Marshal.AllocHGlobal(needed);

            try
            {
                if (!NativeMethods.GetServiceKeyName(handle, displayName, bufferPtr, ref needed))
                {
                    throw new Win32Exception();
                }

                return Marshal.PtrToStringUni(bufferPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }

        public string GetServiceDisplayName(string serviceName)
        {
            Int32 needed = 0;

            if (NativeMethods.GetServiceDisplayName(handle, serviceName, IntPtr.Zero, ref needed))
            {
                throw new ApplicationException($"Unexpected success in {nameof(ServiceControlManager)}.{nameof(GetServiceKeyName)}");
            }

            if (Marshal.GetLastWin32Error() != ERROR_INSUFFICIENT_BUFFER)
            {
                throw new Win32Exception();
            }

            needed = 2 * (needed + 1); // For null terminator and wide chars

            IntPtr bufferPtr = Marshal.AllocHGlobal(needed);

            try
            {
                if (!NativeMethods.GetServiceDisplayName(handle, serviceName, bufferPtr, ref needed))
                {
                    throw new Win32Exception();
                }

                return Marshal.PtrToStringUni(bufferPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseServiceHandle(handle);
        }
    }
}