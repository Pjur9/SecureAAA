using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PrimarnaAGS
{
    public class ReplicationClient : ChannelFactory<IReplicationService>, IReplicationService, IDisposable
    {
        IReplicationService factory;

        public ReplicationClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {

            string cltCertCN = Formater.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;


            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

       
        public void ReplicateFile(List<Alarm> alarms)
        {
            try
            {
                factory.ReplicateFile(alarms);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Communication, Replicate] ERROR = {0}", e.Message);
            }
        }


        public void TransferFile(List<Alarm> alarms)
        {
            try
            {
                factory.TransferFile(alarms);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Communication, Transfer] ERROR = {0}", e.Message);
            }
        }
        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
