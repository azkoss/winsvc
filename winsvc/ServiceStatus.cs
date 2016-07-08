using System.Runtime.InteropServices;
using winsvc.AccessMasks;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace winsvc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ServiceStatus
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(ServiceStatus));
        public SERVICE_TYPE dwServiceType;
        public SERVICE_STATE dwCurrentState;
        public uint dwControlsAccepted;
        public uint dwWin32ExitCode;
        public uint dwServiceSpecificExitCode;
        public uint dwCheckPoint;
        public uint dwWaitHint;
    }
}
