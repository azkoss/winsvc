using System;
using System.Runtime.InteropServices;

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
    }
}
