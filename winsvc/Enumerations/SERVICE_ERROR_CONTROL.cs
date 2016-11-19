// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace winsvc.Enumerations
{
    public enum SERVICE_ERROR_CONTROL : uint
    {
        SERVICE_NO_CHANGE       = 0xFFFFFFFF,
        SERVICE_ERROR_IGNORE   = 0x0000,
        SERVICE_ERROR_NORMAL   = 0x0001,
        SERVICE_ERROR_SEVERE   = 0x0002,
        SERVICE_ERROR_CRITICAL = 0x0003,
    }
}