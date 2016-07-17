# winsvc

A thin wrapper around the Windows service functions.

Extracted from the next version of [Print Distributor](http://www.printdistributor.com). 

Although .Net has some service control facilities I kept coming across gaps and complicating abstractions hence this project. The plan is to cover all the functions and structures described in winsvc.h with a thin wrapper.

No official release yet, it's pending some further development and testing.

Starting a service is as easy as:

```C#
    using (var scm = ServiceControlManager.OpenServiceControlManager(null, (UInt32) SCM_ACCESS.SC_MANAGER_ALL_ACCESS))
    using (scm.OpenService("Your Service Name", (UInt32) SERVICE_ACCESS.SERVICE_ALL_ACCESS))
    {
        service.Start(new string[] {});
    }
```
