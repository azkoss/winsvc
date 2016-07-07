using System;

namespace winsvc
{
    public interface IServiceControlManager : IDisposable
    {
        IService OpenService(string serviceName, UInt32 desiredAccess);
    }
}