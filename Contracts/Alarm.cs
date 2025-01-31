using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Contracts
{
    [DataContract]
    public class Alarm
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public double Risk { get; set; }


        

        public Alarm(int id, string clientName, string message, List<Alarm> alarms)
        {
            Id = id;
            Time = DateTime.Now;
            ClientName = clientName;
            Message = message;
            CalculateRisk(alarms);
        }

        public Alarm(int id, DateTime createdTime, string name, string message, double risk)
        {
            Id = id;
            Time = createdTime;
            ClientName = name;
            Message = message;
            Risk = risk;
        }

        private void CalculateRisk(List<Alarm> alarms)
        {
            double counter = 0;
            switch (Message)
            {
                case "Low Risk":
                    foreach(Alarm a in alarms)
                    {
                        if (a.Message.Equals(Message))
                        {
                            counter++;
                        }
                    }
                    Risk = counter * (15.3 / 100);
                    break;
                case "Medium Risk":
                    foreach (Alarm a in alarms)
                    {
                        if (a.Message.Equals(Message))
                        {
                            counter++;
                        }
                    }
                    Risk = counter * (27.9 / 100);
                    break;
                case "High Risk":
                    foreach (Alarm a in alarms)
                    {
                        if (a.Message.Equals(Message))
                        {
                            counter++;
                        }
                    }
                    Risk = counter * (42.3 / 100);
                    break;
                case "Very High Risk":
                    foreach (Alarm a in alarms)
                    {
                        if (a.Message.Equals(Message))
                        {
                            counter++;
                        }
                    }
                    Risk = counter * (69 / 100);
                    break;
            }
            //Random random = new Random();
            //Risk = random.NextDouble() * 100; // ovde cemo prilagoditi na osnovu kriterijuma koji nam bude odgovarao
        }


        public override string ToString()
        {
            return $"Id: {Id}, Time: {Time}, Client: {ClientName}, Message: {Message}, Risk: {Risk:F2}";
        }
    }
}
