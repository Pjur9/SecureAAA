using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrimarnaAGS
{
    public class WCFService :  ISecurityService
    {
        public static EndpointAddress address = null;
        public static NetTcpBinding binding = null;

        public static void Create(EndpointAddress addr, NetTcpBinding bind)
        {
            address= addr;
            binding = bind;
            List<Alarm> alarms = DataBase.ReadFromCsv();

            using (ReplicationClient proxy = new ReplicationClient(binding, address))
            {
                proxy.TransferFile(alarms);
                Console.WriteLine("Transfering file completed. Press <enter> to continue ...");

            }

        }
        
        public List<Alarm> Read()
        {
            List<Alarm> alarms;
            alarms = DataBase.ReadFromCsv();
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formater.ParseName(principal.Identity.Name);
            try
            {
                Audit.AuthorizationSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return alarms;
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Delete")]
        public bool Delete(int key, string name)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formater.ParseName(principal.Identity.Name);
            if (principal.IsInRole("Delete"))
            {
                try
                {
                    Audit.AuthorizationSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                List<Alarm> alarms = DataBase.ReadFromCsv();
                foreach (Alarm a in alarms)
                {
                    if (a.Id == key)
                    {
                        alarms.Remove(a);
                        DataBase.OverWriteToCsv(alarms);
                        return true;
                    }
                }
                Console.WriteLine($"Korisnik {name} je pokusao da obrise poruku sa nepostojecim id-jem\n");
                return false;
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "Needs Delete permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                

                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Delete method (time: {1}). " +
                    "For this method user needs to be member of group AlarmAdmin.", username, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }

            
        }

        

        
        //[PrincipalPermission(SecurityAction.Demand, Role = "Accept")]
        public bool Accept(string name)
        {
            List<Alarm> alarms = DataBase.ReadFromCsv();
            List<Alarm> alarmi = DataBase.ReadFromCsv();
            int counter = 0;
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formater.ParseName(principal.Identity.Name);
            if (principal.IsInRole("Accept"))
            {
                try
                {
                    Audit.AuthorizationSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                foreach (Alarm a in alarms)
                {
                    if (a.ClientName.Equals(name))
                    {
                        alarmi.Add(a);
                        counter++;
                    }
                }
                if (counter == 0)
                {
                    return false;
                }
                foreach (Alarm a in alarmi)
                {
                    alarms.Remove(a);
                }
                DataBase.OverWriteToCsv(alarms);
                return true;
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "Needs Accept permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Accept method (time: {1}). " +
                    "For this method user needs to be member of group AlarmAdmin.", username, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
            
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Generate")]
        public bool Generate(Alarm a, string name)
        {
            List<Alarm> alarms = new List<Alarm>();
            List<Alarm> alarms1 = DataBase.ReadFromCsv();
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string username = Formater.ParseName(principal.Identity.Name);
            if (principal.IsInRole("Generate"))
            {
                try
                {
                    Audit.AuthorizationSuccess(username, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                foreach (Alarm a1 in alarms1)
                {
                    if (a1.Id.Equals(a.Id))
                    {
                        Console.WriteLine($"Korisnik {name} je pokusao da unese alarm sa postojecim id-jem\n");
                        return false;
                    }
                }
                alarms.Add(a);

                DataBase.WriteToCsv(alarms);
                using (ReplicationClient proxy = new ReplicationClient(binding, address))
                {
                    proxy.ReplicateFile(alarms);
                    Console.WriteLine("ReplicateFile finished. Press <enter> to continue ...");

                }

                return true;
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(username, OperationContext.Current.IncomingMessageHeaders.Action, "Needs Generate permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Generate method (time: {1}). " +
                    "For this method user needs to be member of group AlarmAdmin or AlarmGenerator.", username, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
                
            }
            
                
            
            
        }
    }
}
