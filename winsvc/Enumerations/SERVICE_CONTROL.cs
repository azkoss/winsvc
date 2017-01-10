// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace frogmore.winsvc.Enumerations
{
    public enum SERVICE_CONTROL
    {
        SERVICE_CONTROL_STOP                  = 0x00000001,
        SERVICE_CONTROL_PAUSE                 = 0x00000002,
        SERVICE_CONTROL_CONTINUE              = 0x00000003,
        SERVICE_CONTROL_INTERROGATE           = 0x00000004,
        SERVICE_CONTROL_SHUTDOWN              = 0x00000005,
        SERVICE_CONTROL_PARAMCHANGE           = 0x00000006,
        SERVICE_CONTROL_NETBINDADD            = 0x00000007,
        SERVICE_CONTROL_NETBINDREMOVE         = 0x00000008,
        SERVICE_CONTROL_NETBINDENABLE         = 0x00000009,
        SERVICE_CONTROL_NETBINDDISABLE        = 0x0000000A,
        SERVICE_CONTROL_DEVICEEVENT           = 0x0000000B,
        SERVICE_CONTROL_HARDWAREPROFILECHANGE = 0x0000000C,
        SERVICE_CONTROL_POWEREVENT            = 0x0000000D,
        SERVICE_CONTROL_SESSIONCHANGE         = 0x0000000E,
        SERVICE_CONTROL_PRESHUTDOWN           = 0x0000000F,
        SERVICE_CONTROL_TIMECHANGE            = 0x00000010,
        SERVICE_CONTROL_TRIGGEREVENT          = 0x00000020,
    }
}