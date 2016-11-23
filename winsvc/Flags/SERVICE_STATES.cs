using System;
// ReSharper disable InconsistentNaming

namespace winsvc.Flags
{
    [Flags]
    public enum SERVICE_STATES : uint
    {
        SERVICE_ACTIVE = 0x00000001,
        SERVICE_INACTIVE = 0x00000002,
        SERVICE_STATE_ALL = SERVICE_ACTIVE | SERVICE_INACTIVE
    }
}