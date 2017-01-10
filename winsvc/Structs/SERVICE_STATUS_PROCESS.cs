using System.Runtime.InteropServices;
using frogmore.winsvc.Enumerations;
using frogmore.winsvc.Flags;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

// ReSharper disable InconsistentNaming

namespace frogmore.winsvc.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Unicode)]
    public struct SERVICE_STATUS_PROCESS
    {
        public SERVICE_TYPE serviceType;
        public SERVICE_STATE currentState;
        public uint controlsAccepted;
        public uint win32ExitCode;
        public uint serviceSpecificExitCode;
        public uint checkPoint;
        public uint waitHint;
        public uint processId;
        public uint serviceFlags;
    }
}