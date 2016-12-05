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

        /// <summary>
        /// Creates a service object and adds it to the service control manager database.
        /// </summary>
        /// <param name="serviceName">
        /// The name of the service to install. The maximum string length is 256 characters. The service control manager
        /// database preserves the case of the characters, but service name comparisons are always case insensitive. 
        /// Forward-slash (/) and backslash (\) are not valid service name characters.
        /// </param>
        /// <param name="displayName">
        /// The display name to be used by user interface programs to identify the service. This string has a maximum
        /// length of 256 characters. The name is case-preserved in the service control manager. Display name comparisons
        /// are always case-insensitive.
        /// </param>
        /// <param name="desiredAccess">
        /// The access to the service. Before granting the requested access, the system checks the access token of the
        /// calling process.
        /// </param>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="startType">
        /// The service start options.
        /// </param>
        /// <param name="errorControl">
        /// The severity of the error, and action taken, if this service fails to start.
        /// </param>
        /// <param name="binaryPathName">
        /// The fully qualified path to the service binary file. If the path contains a space, it must be quoted so that 
        /// it is correctly interpreted. For example, "d:\\my share\\myservice.exe" should be specified as "\"d:\\my share\\myservice.exe\"". 
        /// 
        /// The path can also include arguments for an auto-start service. For example, "d:\\myshare\\myservice.exe arg1 arg2". 
        /// These arguments are passed to the service entry point (typically the main function). 
        /// 
        /// If you specify a path on another computer, the share must be accessible by the computer account of the local computer because 
        /// this is the security context used in the remote call. However, this requirement allows any potential vulnerabilities in the 
        /// remote computer to affect the local computer. Therefore, it is best to use a local file.
        /// </param>
        /// <param name="loadOrderGroup">
        /// The names of the load ordering group of which this service is a member. Specify NULL or an empty string if the service does
        /// not belong to a group.
        /// 
        /// The startup program uses load ordering groups to load groups of services in a specified order with respect to the other 
        /// groups. The list of load ordering groups is contained in the following registry value:
        /// 
        /// HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\ServiceGroupOrder
        /// </param>
        /// <param name="tagId">
        /// A pointer to a variable that receives a tag value that is unique in the group specified in the lpLoadOrderGroup parameter. 
        /// Specify NULL if you are not changing the existing tag.
        /// 
        /// You can use a tag for ordering service startup within a load ordering group by specifying a tag order vector in the following
        /// registry value:
        /// 
        /// HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\GroupOrderList
        /// 
        /// Tags are only evaluated for driver services that have SERVICE_BOOT_START or SERVICE_SYSTEM_START start types.
        /// </param>
        /// <param name="dependencies">
        /// The names of services or load ordering groups that the system must start before this service. Specify null if the service has 
        /// no dependencies. Dependency on a group means that this service can run if at least one member of the group is running after
        /// an attempt to start all members of the group.
        /// </param>
        /// <param name="serviceStartName">
        /// The name of the account under which the service should run. If the service type is SERVICE_WIN32_OWN_PROCESS, use an account
        /// name in the form DomainName\UserName. The service process will be logged on as this user. If the account belongs to the 
        /// built-in domain, you can specify .\UserName.
        /// 
        /// If this parameter is null, CreateService uses the LocalSystem account. If the service type specifies SERVICE_INTERACTIVE_PROCESS,
        /// the service must run in the LocalSystem account.
        /// 
        /// If this parameter is NT AUTHORITY\LocalService, CreateService uses the LocalService account. If the parameter is 
        /// NT AUTHORITY\NetworkService, CreateService uses the NetworkService account.
        /// 
        /// A shared process can run as any user.
        /// 
        /// If the service type is SERVICE_KERNEL_DRIVER or SERVICE_FILE_SYSTEM_DRIVER, the name is the driver object name that the system uses
        /// to load the device driver. Specify null if the driver is to use a default object name created by the I/O system.
        /// 
        /// A service can be configured to use a managed account or a virtual account. If the service is configured to use a managed service 
        /// account, the name is the managed service account name. If the service is configured to use a virtual account, specify the name
        /// as NT SERVICE\ServiceName.
        /// 
        /// Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP:  Managed service accounts and virtual accounts are not 
        /// supported until Windows 7 and Windows Server 2008 R2.
        /// </param>
        /// <param name="password">
        /// The password to the account name specified by the lpServiceStartName parameter. Specify an empty string if the account has no 
        /// password or if the service runs in the LocalService, NetworkService, or LocalSystem account.
        /// 
        /// If the account name specified by the lpServiceStartName parameter is the name of a managed service account or virtual account 
        /// name, the lpPassword parameter must be null.
        /// 
        /// Passwords are ignored for driver services.
        /// </param>
        /// <returns>An implementation of IService.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
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

        /// <summary>
        /// Enumerates services in the specified service control manager database. The name and status of each service are provided. 
        /// 
        /// This function has been superseded by the EnumServicesStatusEx function. It returns the same information EnumServicesStatus 
        /// returns, plus the process identifier and additional information for the service. In addition, EnumServicesStatusEx enables 
        /// you to enumerate services that belong to a specified group.
        /// </summary>
        /// <param name="serviceType">The type of services to be enumerated.</param>
        /// <param name="serviceState">The state of the services to be enumerated.</param>
        /// <returns>An enumerator over ENUM_SERVICE_STATUS structures.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        IEnumerable<ENUM_SERVICE_STATUS> EnumServicesStatus(SERVICE_TYPE serviceType, SERVICE_STATE_FLAGS serviceState);

        /// <summary>
        /// Enumerates services in the specified service control manager database.The name and status of each service are provided
        /// </summary>
        /// <param name="serviceType">The type of services to be enumerated.</param>
        /// <param name="serviceStateFlags">The state of the services to be enumerated.</param>
        /// <returns>An enumerator over ENUM_SERVICE_STATUS_PROCESS structures.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        IEnumerable<ENUM_SERVICE_STATUS_PROCESS> EnumServicesStatusEx(SERVICE_TYPE serviceType, SERVICE_STATE_FLAGS serviceStateFlags);

        /// <summary>
        /// Retrieves the service name of the specified service.
        /// 
        /// There are two names for a service: the service name and the display name. The service name is the name of the service's key
        /// in the registry. The display name is a user-friendly name that appears in the Services control panel application, and is
        /// used with the NET START command. Both names are specified with the CreateService function and can be modified with the 
        /// ChangeServiceConfig function. Information specified for a service is stored in a key with the same name as the service 
        /// name under the HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\ServiceName registry key.
        /// 
        /// To map the service name to the display name, use the GetServiceDisplayName function.To map the display name to the service 
        /// name, use the GetServiceKeyName function.
        /// </summary>
        /// <param name="displayName">The service display name.</param>
        /// <returns>The service name</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        string GetServiceKeyName(string displayName);

        string GetServiceDisplayName(string serviceName);
    }
}