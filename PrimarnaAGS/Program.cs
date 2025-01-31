using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Policy;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace PrimarnaAGS
{
    public class Program
    {
        static void Main(string[] args)
        {
            string srvCertCN = "SekundarnaAGS";

            NetTcpBinding bindingWindows = new NetTcpBinding();
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            
            bindingWindows.Security.Mode = SecurityMode.Transport;
            bindingWindows.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingWindows.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            
            string addressWindows = "net.tcp://localhost:9998/Receiver";

            ServiceHost host = new ServiceHost(typeof(WCFService));

            
            host.AddServiceEndpoint(typeof(ISecurityService), bindingWindows, addressWindows);

            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies=policies.AsReadOnly();
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9009/IReplicationService"),
                                      new X509CertificateEndpointIdentity(srvCert));

            WCFService.Create(address, binding);
            Console.WriteLine("Data sent");

            try
            { 
            
                host.Open();
                Console.WriteLine("WCFService is started.\n Press <enter> to stop ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
            }

            


        }
        
    
    }
}
