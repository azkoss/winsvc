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
        void ChangeConfig(
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
        /// Changes the optional configuration parameters of the service.
        /// </summary>
        /// <typeparam name="T">
        /// The type of parameter to set, currently only SERVICE_DESCRIPTION is supported
        /// </typeparam>
        /// <param name="info">
        /// A structure of type T
        /// </param>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        void ChangeConfig2<T>(ref T info);

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
        /// <param name="reason">
        /// Contains service control parameters.
        /// </param>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        void ControlEx(SERVICE_CONTROL control, ref SERVICE_CONTROL_STATUS_REASON_PARAMS reason);

        /// <summary>
        /// Marks the specified service for deletion from the service control manager database.
        /// 
        /// The DeleteService function marks a service for deletion from the service control manager
        /// database. The database entry is not removed until all open handles to the service have been 
        /// closed, and the service is not running.
        /// 
        /// A running service is stopped by a call to the ControlService function with the 
        /// SERVICE_CONTROL_STOP control code. If the service cannot be stopped, the 
        /// database entry is removed when the system is restarted. 
        /// 
        /// The service control manager deletes the service by deleting the service key and its 
        /// subkeys from the registry.
        /// </summary>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        void Delete();

        /// <summary>
        /// Retrieves the name and status of each service that depends on the specified service; that
        /// is, the specified service must be running before the dependent services can run.
        /// </summary>
        /// <param name="states">
        /// The state of the services to be enumerated.
        /// </param>
        /// <returns>Enumeration of services and their state.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        IEnumerable<ENUM_SERVICE_STATUS> EnumDependentServices(SERVICE_STATE_FLAGS states);

        /// <summary>
        /// Retrieves the configuration parameters of the specified service.
        /// </summary>
        /// <returns>QUERY_SERVICE_CONFIG struct.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        QUERY_SERVICE_CONFIG QueryConfig();

        /// <summary>
        /// Retrieves the optional configuration parameters of the service.
        /// </summary>
        /// <typeparam name="T">
        /// The type of information to retrieve from the service, currently only SERVICE_DESCRIPTION is supported.
        /// </typeparam>
        /// <returns>A structure of type T.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        T QueryConfig2<T>();

        /// <summary>
        /// Retrieves the current status of the specified service. This function has been superseded 
        /// by the QueryServiceStatusEx function.QueryServiceStatusEx returns the same information 
        /// QueryServiceStatus returns, with the addition of the process identifier and additional 
        /// information for the service.
        /// </summary>
        /// <returns>SERVICE_STATUS struct.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        SERVICE_STATUS QueryStatus();

        /// <summary>
        /// Retrieves the current status of the specified service
        /// </summary>
        /// <returns>SERVICE_STATUS_PROCESS struct.</returns>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        SERVICE_STATUS_PROCESS QueryStatusEx();

        /// <summary>
        /// Starts a service.
        /// </summary>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        void Start();

        /// <summary>
        /// Starts a service
        /// </summary>
        /// <param name="serviceArgVectors">
        /// The strings to be passed to the ServiceMain function for the service as arguments. If 
        /// there are no arguments, this parameter can be NULL. Otherwise, the first argument 
        /// (lpServiceArgVectors[0]) is the name of the service, followed by any additional 
        /// arguments (lpServiceArgVectors[1] through lpServiceArgVectors[dwNumServiceArgs-1])
        /// </param>
        /// <exception cref="Win32Exception">Thrown if the underlying API call fails.</exception>
        void Start(string[] serviceArgVectors);
    }
}