// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace winsvc.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Unicode)]
    public struct SERVICE_DESCRIPTION
    {
        public string Description;
    }
}
