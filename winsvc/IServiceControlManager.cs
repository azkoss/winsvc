using System;
using System.Collections.Generic;
using winsvc.Flags;
using winsvc.Structs;

namespace winsvc
{
    public interface IServiceControlManager : IDisposable
    {
        IService OpenService(string serviceName, SERVICE_ACCESS desiredAccess);
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

        IEnumerable<ENUM_SERVICE_STATUS> EnumServicesStatus();
    }
}