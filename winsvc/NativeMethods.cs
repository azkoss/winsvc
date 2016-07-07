using System;
using System.Runtime.InteropServices;

namespace winsvc
{
    internal static class NativeMethods
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr OpenSCManager(string machineName, string databaseName, UInt32 desiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool CloseServiceHandle(IntPtr SCObject);
    }
}
