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

        /// <summary>
        /// Changes the configuration parameters of a service.
        /// </summary>
        /// <param name="serviceType">
        /// The type of service. Specify SERVICE_NO_CHANGE if you are not changing the existing service type.
        /// </param>
        /// <param name="startType">
        /// The service start options. Specify SERVICE_NO_CHANGE if you are not changing the existing start type.
        /// </param>
        /// <param name="errorControl">
        /// The severity of the error, and action taken, if this service fails to start. Specify SERVICE_NO_CHANGE
        /// if you are not changing the existing error control.
        /// </param>
        /// <param name="binaryPathName">
        /// The fully qualified path to the service binary file. Specify null if you are not changing the existing 
        /// path. If the path contains a space, it must be quoted so that it is correctly interpreted. For 
        /// example, "d:\\my share\\myservice.exe" should be specified as "\"d:\\my share\\myservice.exe\""
        /// 
        /// The path can also include arguments for an auto-start service. For example, 
        /// "d:\\myshare\\myservice.exe arg1 arg2". These arguments are passed to the service entry point 
        /// (typically the main function).
        /// 
        /// If you specify a path on another computer, the share must be accessible by the computer account 
        /// of the local computer because this is the security context used in the remote call. However, 
        /// this requirement allows any potential vulnerabilities in the remote computer to affect the 
        /// local computer. Therefore, it is best to use a local file.
        /// </param>
        /// <param name="loadOrderGroup">
        /// The name of the load ordering group of which this service is a member. Specify null if you are 
        /// not changing the existing group. Specify an empty string if the service does not belong to a group.
        /// </param>
        /// <param name="tagId">
        /// A pointer to a variable that receives a tag value that is unique in the group specified in the 
        /// lpLoadOrderGroup parameter. Specify null if you are not changing the existing tag.
        /// </param>
        /// <param name="dependencies">
        /// Enumerable names of services or load ordering groups that the system must start before this 
        /// service can be started. (Dependency on a group means that this service can run if at least 
        /// one member of the group is running after an attempt to start all members of the group.) 
        /// Specify null if you are not changing the existing dependencies. Specify an empty collection 
        /// if the service has no dependencies.
        /// </param>
        /// <param name="serviceStartName">
        /// The name of the account under which the service should run. Specify null if you are not 
        /// changing the existing account name. If the service type is SERVICE_WIN32_OWN_PROCESS, 
        /// use an account name in the form DomainName\UserName. The service process will be logged
        ///  on as this user. If the account belongs to the built-in domain, you can 
        /// specify .\UserName (note that the corresponding C/C++ string is ".\\UserName").
        /// </param>
        /// <param name="password">
        /// The password to the account name specified by the lpServiceStartName parameter. Specify null
        /// if you are not changing the existing password. Specify an empty string if the account has
        /// no password or if the service runs in the LocalService, NetworkService, or LocalSystem 
        /// account.
        /// </param>
        /// <param name="displayName">
        /// The display name to be used by applications to identify the service for its users. Specify 
        /// null if you are not changing the existing display name; otherwise, this string has a 
        /// maximum length of 256 characters. The name is case-preserved in the service control 
        /// manager. Display name comparisons are always case-insensitive.
        /// </param>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
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

        /// <summary>
        /// Sends a control code to a service.
        /// 
        /// The ControlService function asks the Service Control Manager (SCM) to send the requested control 
        /// code to the service. The SCM sends the code if the service has specified that it will accept 
        /// the code, and is in a state in which a control code can be sent to it.
        /// </summary>
        /// <param name="control">
        /// The control code.
        /// </param>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
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