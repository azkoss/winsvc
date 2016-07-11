using System;
using winsvc.AccessMasks;
using winsvc.Structs;

namespace winsvc
{
    public interface IService : IDisposable
    {
        void Delete();
        void Start(string[] serviceArgVectors);
        void Control(SERVICE_CONTROL control, ref SERVICE_STATUS status);
    }
}