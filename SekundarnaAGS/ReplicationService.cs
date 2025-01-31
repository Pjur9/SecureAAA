using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
namespace SekundarnaAGS
{
    public class ReplicationService : IReplicationService
    {
      
        public void ReplicateFile(List<Alarm> alarms)
        {
            int counter = 0;
            List<Alarm> before = DataBase2.ReadFromCsv();
            for(int i = 0; i < alarms.Count; i++)
            {
                for(int j = 0; j < before.Count; j++)
                {
                    //Console.WriteLine(alarms[i].ToString());
                    //Console.WriteLine(before[j].ToString());
                    if (alarms[i].Id == before[j].Id)
                    {
                        Console.WriteLine($"Alarm {before[j]} ce biti zamjenjen sa {alarms[i]}\n");
                        before[j]=alarms[i];
                        DataBase2.OverWriteToCsv(before);
                        counter++;
                    }
                }
            }
            if (counter == 0)
            {
                DataBase2.WriteToCsv(alarms);
                Console.WriteLine("Uspjesno unijeti podaci u bazu\n");
            }
            
            List<Alarm> alarmList = DataBase2.ReadFromCsv();
            foreach (Alarm alarm in alarmList)
            {
                Console.WriteLine(alarm.ToString());
            }
        }

        public void TransferFile(List<Alarm> alarms)
        {

            DataBase2.OverWriteToCsv(alarms);
            Console.WriteLine("Transferovanje podataka iz PrimarneAGS u SekundarnuAGS uspjesno!\n");
            List<Alarm> alarmList = DataBase2.ReadFromCsv();
            foreach (Alarm alarm in alarmList)
            {
                Console.WriteLine(alarm.ToString());
            }
        }
    }
}
