using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9998/Receiver";
            
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            string name = Formater.ParseName(WindowsIdentity.GetCurrent().Name);
            List<Alarm> alarms = new List<Alarm>();
            string user = WindowsIdentity.GetCurrent().Name;
            user = user.Split('\\')[1];
            Console.WriteLine($"Prijavljeni korisnik:{user}");
            while (true)
            {
                Console.WriteLine("Izaberite jednu od sledecih opcija:");
                Console.WriteLine("1.Read");
                Console.WriteLine("2.Generate");
                Console.WriteLine("3.Delete");
                Console.WriteLine("4.Accept");
                int num=int.Parse(Console.ReadLine());
                

                using (ClientProxy proxy = new ClientProxy(binding, address))
                {
                    
                    switch (num)
                    {
                        case 1:
                            alarms = proxy.Read();
                            foreach (Alarm al in alarms)
                            {
                                Console.WriteLine(al.ToString());
                            }
                            break;
                        case 2:
                            alarms = proxy.Read();
                            Console.WriteLine("Unesite alarm:\n");
                            Console.Write("ID:");
                            int id = int.Parse(Console.ReadLine());
                            Console.Write("\n1.Low Risk:");
                            Console.Write("\n2.Medium Risk:");
                            Console.Write("\n3.High Risk:");
                            Console.Write("\n4.Very High Risk:");
                            Console.Write("\nUnesi tip poruke:");
                            int poruka = int.Parse(Console.ReadLine());
                            string message = "";
                            switch (poruka)
                            {
                                case 1:
                                    message = "Low Risk";
                                    break;
                                case 2:
                                    message = "Medium Risk";
                                    break;
                                case 3:
                                    message = "High Risk";
                                    break;
                                case 4:
                                    message = "Very High Risk";
                                    break;
                            }
                            Alarm alarm = new Alarm(id, name, message, alarms);
                            bool gen = proxy.Generate(alarm, name);
                            if (!gen)
                            {
                                Console.WriteLine("Unjeli ste alarm sa postojecim id-jem, molimo izaberite opciju 1. da provjerite listu sa postojecim alarmima\n");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Unesi id alarma:");
                            id = Int32.Parse(Console.ReadLine());
                            bool del = proxy.Delete(id, name);
                            if (!del)
                            {
                                Console.WriteLine("Ne postoji alarm sa datim id-jem, molimo izaberite opciju 1. da provjerite listu sa postojecim alarmima\n");
                            }
                            break;
                        case 4:
                            proxy.Accept(name);
                            break;     
                    }
                }
            }
        }
    }
}
