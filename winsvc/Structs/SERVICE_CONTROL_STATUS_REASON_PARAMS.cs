using System.Runtime.InteropServices;
using winsvc.Flags;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace winsvc.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Unicode)]
    public struct SERVICE_CONTROL_STATUS_REASON_PARAMS
    {
        public SERVICE_STOP_REASON reason;
        public string comment;
        public SERVICE_STATUS_PROCESS serviceStatus;
    }
}