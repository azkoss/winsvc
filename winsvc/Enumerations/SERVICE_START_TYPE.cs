using System;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace winsvc.Enumerations
{
    [Flags]
    public enum SERVICE_START_TYPE : uint
    {
        SERVICE_NO_CHANGE   = 0xFFFFFFFF,
        SERVICE_BOOT_START   = 0x0000,
        SERVICE_SYSTEM_START = 0x0001,
        SERVICE_AUTO_START   = 0x0002,
        SERVICE_DEMAND_START = 0x0003,
        SERVICE_DISABLED     = 0x0004
    }
}