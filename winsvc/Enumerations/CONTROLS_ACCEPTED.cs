using System;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace winsvc.Enumerations
{
    [Flags]
    internal enum CONTROLS_ACCEPTED : uint
    {
        SERVICE_ACCEPT_NETBINDCHANGE         = 0x00000010,
        SERVICE_ACCEPT_PARAMCHANGE           = 0x00000008,
        SERVICE_ACCEPT_PAUSE_CONTINUE        = 0x00000002,
        SERVICE_ACCEPT_PRESHUTDOWN           = 0x00000100,
        SERVICE_ACCEPT_SHUTDOWN              = 0x00000004,
        SERVICE_ACCEPT_STOP                  = 0x00000001,

        //supported only by HandlerEx
        SERVICE_ACCEPT_HARDWAREPROFILECHANGE = 0x00000020,
        SERVICE_ACCEPT_POWEREVENT            = 0x00000040,
        SERVICE_ACCEPT_SESSIONCHANGE         = 0x00000080,
        SERVICE_ACCEPT_TIMECHANGE            = 0x00000200,
        SERVICE_ACCEPT_TRIGGEREVENT          = 0x00000400,
        SERVICE_ACCEPT_USERMODEREBOOT        = 0x00000800
    }
}