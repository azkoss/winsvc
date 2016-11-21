using System;
using System.Collections.Generic;
using winsvc.Enumerations;
using winsvc.Flags;
using winsvc.Structs;

namespace winsvc
{
    public interface IServiceControlManager : IDisposable
    {
        IService OpenService(string serviceName, SERVICE_ACCESS desiredAccess);
        IService CreateService(string serviceName,
                                string displayName,
                                SERVICE_ACCESS desiredAccess,
                                SERVICE_TYPE serviceType,
                                SERVICE_START_TYPE startType,
                                SERVICE_ERROR_CONTROL errorControl,
                                string binaryPathName,
                                string loadOrderGroup,
                                IntPtr tagId,
                                string dependencies,
                                string serviceStartName,
                                string password);

        IEnumerable<ENUM_SERVICE_STATUS> EnumServicesStatus();
    }
}