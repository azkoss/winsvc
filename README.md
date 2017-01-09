# winsvc

A thin wrapper around the Windows service functions.

Extracted from the next version of [Print Distributor](http://www.printdistributor.com). 

Although .Net has some service control facilities I kept coming across gaps and complicating abstractions hence this project.

Note this project does not and will not allow you to create a service, only control and manage services.

Starting a service is as easy as:

```C#
    using (var scm = ServiceControlManager.OpenServiceControlManager(null, SCM_ACCESS.SC_MANAGER_ALL_ACCESS))
    using (var service = scm.OpenService("Your Service Name", SERVICE_ACCESS.SERVICE_ALL_ACCESS))
    {
        service.Start();
    }
```
