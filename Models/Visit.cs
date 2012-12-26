using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace RealTimeWebAnalytics.Models
{
    public class Visit : IEvent
    {
        public Visit()
        {
            EventId = Guid.NewGuid();
        }

        [JsonIgnore]
        public IPAddress IP { get; set; }
        public string IPStr { get { return IP.ToString(); } }
        public string Content { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        
        [JsonIgnore]
        public Guid EventId { get; private set; }
        [JsonIgnore]
        public string Type { get { return typeof (Visit).FullName; } }
        [JsonIgnore]
        public bool IsJson { get { return true; } }
        [JsonIgnore]
        public byte[] Data { get { return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this)); } }
        [JsonIgnore]
        public byte[] Metadata { get { return new byte[0]; } }
    }
}