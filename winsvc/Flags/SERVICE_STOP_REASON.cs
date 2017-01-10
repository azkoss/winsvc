using System;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace frogmore.winsvc.Flags
{
    [Flags]
    public enum SERVICE_STOP_REASON : uint
    {
        FLAG_MIN                           = 0x00000000,
        FLAG_UNPLANNED                     = 0x10000000,
        FLAG_CUSTOM                        = 0x20000000,
        FLAG_PLANNED                       = 0x40000000,
        FLAG_MAX                           = 0x80000000,

        //
        // Microsoft major reasons. Update SERVICE_STOP_REASON_MAJOR_MAX when
        // new codes are added.
        //
        MAJOR_MIN                           = 0x00000000,
        MAJOR_OTHER                         = 0x00010000,
        MAJOR_HARDWARE                      = 0x00020000,
        MAJOR_OPERATINGSYSTEM               = 0x00030000,
        MAJOR_SOFTWARE                      = 0x00040000,
        MAJOR_APPLICATION                   = 0x00050000,
        MAJOR_NONE                          = 0x00060000,
        MAJOR_MAX                           = 0x00070000,
        MAJOR_MIN_CUSTOM                    = 0x00400000,
        MAJOR_MAX_CUSTOM                    = 0x00ff0000,

        //
        // Microsoft minor reasons. Update SERVICE_STOP_REASON_MINOR_MAX when
        // new codes are added.
        //
        MINOR_MIN                           = 0x00000000,
        MINOR_OTHER                         = 0x00000001,
        MINOR_MAINTENANCE                   = 0x00000002,
        MINOR_INSTALLATION                  = 0x00000003,
        MINOR_UPGRADE                       = 0x00000004,
        MINOR_RECONFIG                      = 0x00000005,
        MINOR_HUNG                          = 0x00000006,
        MINOR_UNSTABLE                      = 0x00000007,
        MINOR_DISK                          = 0x00000008,
        MINOR_NETWORKCARD                   = 0x00000009,
        MINOR_ENVIRONMENT                   = 0x0000000a,
        MINOR_HARDWARE_DRIVER               = 0x0000000b,
        MINOR_OTHERDRIVER                   = 0x0000000c,
        MINOR_SERVICEPACK                   = 0x0000000d,
        MINOR_SOFTWARE_UPDATE               = 0x0000000e,
        MINOR_SECURITYFIX                   = 0x0000000f,
        MINOR_SECURITY                      = 0x00000010,
        MINOR_NETWORK_CONNECTIVITY          = 0x00000011,
        MINOR_WMI                           = 0x00000012,
        MINOR_SERVICEPACK_UNINSTALL         = 0x00000013,
        MINOR_SOFTWARE_UPDATE_UNINSTALL     = 0x00000014,
        MINOR_SECURITYFIX_UNINSTALL         = 0x00000015,
        MINOR_MMC                           = 0x00000016,
        MINOR_NONE                          = 0x00000017,
        MINOR_MAX                           = 0x00000018,
        MINOR_MIN_CUSTOM                    = 0x00000100,
        MINOR_MAX_CUSTOM                    = 0x0000FFFF,
    }
}