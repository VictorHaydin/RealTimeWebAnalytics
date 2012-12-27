using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using RealTimeWebAnalytics.Models;

namespace RealTimeWebAnalytics.Services
{
    public class StorageService : IDisposable
    {
        private readonly EventStoreConnection _connection;

        private StorageService()
        {
            _connection = EventStoreConnection.Create();
            _connection.Connect(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), 1113)); // todo: configure it
        }

        public static void Init()
        {
            Instance = new StorageService();
        }

        public static StorageService Instance { get; private set; }

        public async Task StoreVisit(Visit visit)
        {
            await _connection.AppendToStreamAsync("Visits", -2, new IEvent[] { visit });
        }

        public async Task<IEnumerable<Visit>> GetAllVisits()
        {
            var slice = _connection.ReadEventStreamForward("Visits", 0, int.MaxValue);
            return slice.Events.Select(e => JsonConvert.DeserializeObject<Visit>(Encoding.UTF8.GetString(e.Data))).Where(v => v != null).ToArray();
        }
            
        public void Dispose()
        {
            _connection.Close();
        }
    }
}