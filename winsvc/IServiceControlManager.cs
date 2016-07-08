using System;

namespace winsvc
{
    public interface IServiceControlManager : IDisposable
    {
        IService OpenService(string serviceName, UInt32 desiredAccess);
        IService CreateService(string serviceName,
                                string displayName,
                                UInt32 desiredAccess,
                                UInt32 serviceType,
                                UInt32 startType,
                                UInt32 errorControl,
                                string binaryPathName,
                                string loadOrderGroup,
                                IntPtr tagId,
                                string dependencies,
                                string serviceStartName,
                                string password);
    }
}