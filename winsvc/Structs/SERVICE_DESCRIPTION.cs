// ReSharper disable InconsistentNaming

using System.Runtime.InteropServices;

namespace frogmore.winsvc.Structs
{
    [StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Unicode)]
    public struct SERVICE_DESCRIPTION
    {
        public string Description;
    }
}
