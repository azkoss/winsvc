using System;
using System.Collections.Generic;
using winsvc.Enumerations;
using winsvc.Flags;
using winsvc.Structs;

#pragma warning disable 1584,1711,1572,1581,1580

namespace winsvc
{
    public interface IService : IDisposable
    {
        /// <summary>
        /// Get or set the description  of the service.
        /// </summary>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        string Description { get; set; }

        void ChangeServiceConfig(
            SERVICE_TYPE       serviceType,
            SERVICE_START_TYPE       startType,
            SERVICE_ERROR_CONTROL       errorControl,
            string     binaryPathName,
            string     loadOrderGroup,
            IntPtr     tagId,
            IEnumerable<string>     dependencies,
            string     serviceStartName,
            string     password,
            string     displayName
        );

        void Control(SERVICE_CONTROL control);

        void ControlEx(SERVICE_CONTROL control, ref SERVICE_CONTROL_STATUS_REASON_PARAMS reason);

        void Delete();

        IEnumerable<ENUM_SERVICE_STATUS> EnumDependentServices(SERVICE_STATE_FLAGS states);

        QUERY_SERVICE_CONFIG QueryServiceConfig();

        SERVICE_STATUS QueryServiceStatus();

        SERVICE_STATUS_PROCESS QueryServiceStatusEx();

        void Start();
        void Start(string[] serviceArgVectors);
    }
}