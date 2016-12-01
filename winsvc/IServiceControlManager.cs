using System;
using System.Collections.Generic;
using winsvc.Enumerations;
using winsvc.Flags;
using winsvc.Structs;

#pragma warning disable 1584,1711,1572,1581,1580

namespace winsvc
{
    /// <summary>  
    ///  Interface for service control manager.  
    /// </summary> 
    public interface IServiceControlManager : IDisposable
    {
        /// <summary>  
        /// Opens an existing service.  
        /// </summary> 
        /// <param name="serviceName">
        /// The name of the service to be opened. This is the name specified by the lpServiceName parameter of the CreateService
        /// function when the service object was created, not the service display name that is shown by user interface applications to identify the service.
        /// 
        /// The maximum string length is 256 characters.The service control manager database preserves the case of the characters, but service name comparisons
        /// are always case insensitive. Forward-slash(/) and backslash(\) are invalid service name characters.
        /// </param>
        /// <param name="desiredAccess">
        /// The access to the service.
        /// Before granting the requested access, the system checks the access token of the calling process against the discretionary access-control
        /// list of the security descriptor associated with the service object.
        /// </param>
        /// <returns>An implementation of IService.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
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
                                ICollection<string> dependencies,
                                string serviceStartName,
                                string password);

        IEnumerable<ENUM_SERVICE_STATUS> EnumServicesStatus();
        IEnumerable<ENUM_SERVICE_STATUS_PROCESS> EnumServicesStatusEx();

        string GetServiceKeyName(string displayName);
        string GetServiceDisplayName(string serviceName);
    }
}