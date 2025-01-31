using Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SekundarnaAGS
{
    public class DataBase2
    {
        public static void WriteToCsv(List<Alarm> alarms)
        {
            try
            {
                string path = "C:\\Users\\User\\OneDrive\\Desktop\\Baza\\BazaReplikacija.csv";
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    foreach (Alarm alarm in alarms)
                    {
                        string line = $"{alarm.Id},{alarm.Time.ToString("dd-MM-yyyy HH:mm:ss")},{alarm.ClientName},{alarm.Message},{alarm.Risk}";
                        sw.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void OverWriteToCsv(List<Alarm> alarms)
        {
            try
            {
                string path = "C:\\Users\\User\\OneDrive\\Desktop\\Baza\\BazaReplikacija.csv";
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (Alarm alarm in alarms)
                    {
                        string line = $"{alarm.Id},{alarm.Time.ToString("dd-MM-yyyy HH:mm:ss")},{alarm.ClientName},{alarm.Message},{alarm.Risk}";
                        sw.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static List<Alarm> ReadFromCsv()
        {
            List<Alarm> alarms = new List<Alarm>();
            string path = "C:\\Users\\User\\OneDrive\\Desktop\\Baza\\BazaReplikacija.csv";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] row = sr.ReadLine().Split(',');
                        if (row.Length == 5)
                        {
                            Alarm alarm = new Alarm(Int32.Parse(row[0]), DateTime.ParseExact(row[1], "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture), row[2], row[3], Double.Parse(row[4]));
                            alarms.Add(alarm);
                        }
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("GRESKA" + e.Message);
            }

            return alarms;
        }
    }
}
