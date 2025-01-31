using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SecurityException = System.Security.SecurityException;

namespace Client
{
    
        public class ClientProxy : ChannelFactory<ISecurityService>, ISecurityService, IDisposable
        {
            ISecurityService factory;

            public ClientProxy(NetTcpBinding binding, string address) : base(binding, address)
            {
                factory = this.CreateChannel();
            }

        
        public List<Alarm> Read()
        {
            List<Alarm> alarms = new List<Alarm>();
            try
            {
                alarms = factory.Read();
                Console.WriteLine("Read allowed");
            }
            catch (FaultException<Contracts.SecurityException> e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Detail.Message);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Read : {0}", e.Message);
                
            }
            return alarms;
        }

        public bool Delete(int key, string name)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Delete(key, name);
                Console.WriteLine("Delete allowed");
            }
            catch (FaultException<Contracts.SecurityException> e)
            {
                Console.WriteLine("Error while trying to Delete : {0}", e.Detail.Message);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Delete : {0}", e.Message);
                return true;
            }
            return retValue;
        }


        public bool Accept(string name)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Accept(name);
                Console.WriteLine("Accept allowed");
            }
            catch (FaultException<Contracts.SecurityException> e)
            {
                Console.WriteLine("Error while trying to Accept  : {0}", e.Detail.Message);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Accept : {0}", e.Message);
                return true;
            }
            return retValue;
        }

        public bool Generate(Alarm a, string name)
        {
            bool retValue = false;
            try
            {
                retValue = factory.Generate(a, name );
                Console.WriteLine("Generate allowed");
            }
            catch (FaultException<Contracts.SecurityException> e)
            {
                Console.WriteLine("Error while trying to Generate  : {0}", e.Detail.Message);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Generate : {0}", e.Message);
                return true;
            }
            return retValue;
        }
    }
    }

