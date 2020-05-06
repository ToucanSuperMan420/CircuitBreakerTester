using System.ServiceModel;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string DoWork();

        [OperationContract]
        void SetIsAlive(bool IsAlive);
    }
}