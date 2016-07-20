﻿using System;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace winsvc.Flags
{
    [Flags]
    public enum SCM_ACCESS : uint
    {
        SC_MANAGER_CONNECT            = 0x00000001,
        SC_MANAGER_CREATE_SERVICE     = 0x00000002,
        SC_MANAGER_ENUMERATE_SERVICE  = 0x00000004,
        SC_MANAGER_LOCK               = 0x00000008,
        SC_MANAGER_QUERY_LOCK_STATUS  = 0x00000010,
        SC_MANAGER_MODIFY_BOOT_CONFIG = 0x00000020,
        SC_MANAGER_ALL_ACCESS         = 0x000F003F,
    }
}