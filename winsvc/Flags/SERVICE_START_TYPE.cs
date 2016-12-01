using System;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace winsvc.Flags
{
    [Flags]
    public enum SERVICE_START_TYPE : uint
    {
        SERVICE_NO_CHANGE    = 0xFFFFFFFF,
        SERVICE_BOOT_START   = 0x00000000,
        SERVICE_SYSTEM_START = 0x00000001,
        SERVICE_AUTO_START   = 0x00000002,
        SERVICE_DEMAND_START = 0x00000003,
        SERVICE_DISABLED     = 0x00000004
    }
}