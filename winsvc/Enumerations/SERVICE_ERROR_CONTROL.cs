// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace frogmore.winsvc.Enumerations
{
    public enum SERVICE_ERROR_CONTROL : uint
    {
        SERVICE_NO_CHANGE      = 0xFFFFFFFF,
        SERVICE_ERROR_IGNORE   = 0x00000000,
        SERVICE_ERROR_NORMAL   = 0x00000001,
        SERVICE_ERROR_SEVERE   = 0x00000002,
        SERVICE_ERROR_CRITICAL = 0x00000003,
    }
}