using System;
using System.Runtime.InteropServices;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace winsvc.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Unicode)]
    public struct QUERY_SERVICE_CONFIG {
        UInt32 ServiceType;
        UInt32 StartType;
        UInt32 ErrorControl;
        string BinaryPathName;
        string LoadOrderGroup;
        UInt32 TagId;
        string Dependencies;
        string ServiceStartName;
        public string DisplayName;
    }
}