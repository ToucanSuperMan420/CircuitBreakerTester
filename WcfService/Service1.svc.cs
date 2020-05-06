using System.ServiceModel;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private static bool _isAlive = true;

        public string DoWork()
        {
            return _isAlive ? "Connexion Successfull" : throw new EndpointNotFoundException();
        }

        public void SetIsAlive(bool IsAlive)
        {
            _isAlive = IsAlive;
        }
    }
}