using System;
using winsvc.Enumerations;
using winsvc.Flags;
using winsvc.Structs;

namespace winsvc
{
    public interface IService : IDisposable
    {
        void Delete();
        void Start(string[] serviceArgVectors);

        void Control(SERVICE_CONTROL control);

        QUERY_SERVICE_CONFIG QueryServiceConfig();

        void ChangeServiceConfig(
            SERVICE_TYPE       serviceType,
            SERVICE_START_TYPE       startType,
            SERVICE_ERROR_CONTROL       errorControl,
            string     binaryPathName,
            string     loadOrderGroup,
            IntPtr     tagId,
            string     dependencies,
            string     serviceStartName,
            string     password,
            string     displayName
        );

        SERVICE_STATUS QueryServiceStatus();

        string Description { get; set; }
    }
}