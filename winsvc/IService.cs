using System;
using System.Collections.Generic;
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
        void ControlEx(SERVICE_CONTROL control, ref SERVICE_CONTROL_STATUS_REASON_PARAMS reason);

        QUERY_SERVICE_CONFIG QueryServiceConfig();

        void ChangeServiceConfig(
            SERVICE_TYPE       serviceType,
            SERVICE_START_TYPE       startType,
            SERVICE_ERROR_CONTROL       errorControl,
            string     binaryPathName,
            string     loadOrderGroup,
            IntPtr     tagId,
            string     dependencies, // TODO Change to IEnumerable<string>
            string     serviceStartName,
            string     password,
            string     displayName
        );

        SERVICE_STATUS QueryServiceStatus();

        SERVICE_STATUS_PROCESS QueryServiceStatusEx();

        IEnumerable<ENUM_SERVICE_STATUS> EnumDependentServices(SERVICE_STATES states);

        string Description { get; set; }
    }
}