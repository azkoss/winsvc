using System;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace frogmore.winsvc.Flags
{
    [Flags]
    public enum SERVICE_TYPE : uint
    {
        SERVICE_NO_CHANGE           = 0xFFFFFFFF,
        SERVICE_KERNEL_DRIVER       = 0x0001,
        SERVICE_FILE_SYSTEM_DRIVER  = 0x0002,
        SERVICE_ADAPTER             = 0x0004,
        SERVICE_RECOGNIZER_DRIVER   = 0x0008,
        SERVICE_WIN32_OWN_PROCESS   = 0x0010,
        SERVICE_WIN32_SHARE_PROCESS = 0x0020,

        SERVICE_WIN32 = SERVICE_WIN32_OWN_PROCESS | SERVICE_WIN32_SHARE_PROCESS, // 0x0030

        SERVICE_INTERACTIVE_PROCESS = 0x0100,
    }
}