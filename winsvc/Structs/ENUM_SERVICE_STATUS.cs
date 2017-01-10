using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace frogmore.winsvc.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Unicode)]
    public struct ENUM_SERVICE_STATUS
    {
        public string ServiceName;
        public string DisplayName;
        public SERVICE_STATUS ServiceStatus;

        public override string ToString()
        {
            return ServiceName;
        }
    }
}