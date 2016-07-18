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
        public UInt32 ServiceType;
        public UInt32 StartType;
        public UInt32 ErrorControl;
        public string BinaryPathName;
        public string LoadOrderGroup;
        public UInt32 TagId;
        public string Dependencies;
        public string ServiceStartName;
        public string DisplayName;
    }
}