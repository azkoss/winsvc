using System;
using System.Runtime.InteropServices;
using winsvc.Enumerations;
using winsvc.Flags;
using winsvc.Structs;

namespace winsvc
{
    // Conventions

    // All declarations are Unicode
    // BOOL are marshalled as bool
    // HANDLEs are marshalled as UIntPtr
    // DWORDs are marshalled as Int32

    internal static class NativeMethods
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenSCManager(string machineName, string databaseName, UInt32 desiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CloseServiceHandle(IntPtr serviceControlObject);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenService(IntPtr serviceControlObject, string serviceName, UInt32 desiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateService(
            IntPtr serviceConrolObject,
            string serviceName,
            string displayName,
            UInt32 desiredAccess,
            UInt32 serviceType,
            UInt32 startType,
            UInt32 errorControl,
            string binaryPathName,
            string loadOrderGroup,
            IntPtr tagId,
            string dependencies,
            string serviceStartName,
            string password);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DeleteService(IntPtr serviceHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool StartService(IntPtr serviceHandle, int argCount, string[] argVectors);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool ControlService(IntPtr serviceHandle, SERVICE_CONTROL control, ref SERVICE_STATUS serviceStatus);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool EnumServicesStatus(
            IntPtr serviceControlObject, 
            SERVICE_TYPE serviceType, 
            SERVICE_STATE_FLAGS serviceState, 
            IntPtr bufferPtr, 
            Int32 bufferSize, 
            ref Int32 bufferNeeded, 
            ref Int32 servicesReturned, 
            ref UInt32 resumeHandle);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool EnumServicesStatusEx(IntPtr hSCManager,
            int infoLevel, 
            SERVICE_TYPE serviceType,
            SERVICE_STATE_FLAGS serviceState, 
            IntPtr services, 
            Int32 bufSize,
            ref Int32 bytesNeeded, 
            ref Int32 servicesReturned,
            ref UInt32 resumeHandle, 
            string groupName
        );

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool EnumDependentServices(
            IntPtr serviceHandle,
            SERVICE_STATES serviceState,
            IntPtr bufferPtr,
            Int32 bufferSize,
            ref Int32 bufferNeeded,
            ref Int32 servicesReturned);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool QueryServiceConfig(
            IntPtr serviceHandle, 
            IntPtr bufferPtr, 
            Int32 bufferSize, 
            ref Int32 bufferNeeded);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ChangeServiceConfig(
            IntPtr hService, 
            UInt32 nServiceType, 
            UInt32 nStartType, 
            UInt32 nErrorControl, 
            String lpBinaryPathName, 
            String lpLoadOrderGroup, 
            IntPtr lpdwTagId, 
            String lpDependencies, 
            String lpServiceStartName, 
            String lpPassword, 
            String lpDisplayName);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ChangeServiceConfig2(
            IntPtr hService,
            int dwInfoLevel,
            [MarshalAs(UnmanagedType.Struct)] ref SERVICE_DESCRIPTION lpInfo
        );

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool QueryServiceConfig2(
            IntPtr hService, 
            UInt32 dwInfoLevel, 
            IntPtr buffer, 
            Int32 cbBufSize, 
            ref Int32 pcbBytesNeeded
        );

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool QueryServiceStatus(IntPtr hService, ref SERVICE_STATUS dwServiceStatus);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool QueryServiceStatusEx(
            IntPtr serviceHandle, 
            UInt32 infoLevel, 
            IntPtr buffer, 
            Int32 bufferSize, 
            ref Int32 bytesNeeded
        );
    }
}