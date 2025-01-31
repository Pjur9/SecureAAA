using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IReplicationService
    {
        [OperationContract]
        void ReplicateFile(List<Alarm> alarms);

        [OperationContract]
        void TransferFile(List<Alarm> alarms);
    }
}
