using System;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace winsvc.AccessMasks
{
    [Flags]
    public enum SERVICE_STATE_ENUM : uint
    {
        SERVICE_ACTIVE = 0x00000001,
        SERVICE_INACTIVE = 0x00000002,
        SERVICE_STATE_ALL = SERVICE_ACTIVE | SERVICE_INACTIVE, // 0x00000003
    }
}