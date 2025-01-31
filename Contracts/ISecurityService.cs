using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        [FaultContract(typeof(SecurityException))]

        List<Alarm> Read();

        [OperationContract]
        [FaultContract(typeof(SecurityException))]

        bool Delete(int id, string name);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]

        bool Accept(string name);


        [OperationContract]
        [FaultContract(typeof(SecurityException))]

        bool Generate(Alarm a, string name);
        
    }
}
