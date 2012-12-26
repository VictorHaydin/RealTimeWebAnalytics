using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using EventStore.ClientAPI;
using RealTimeWebAnalytics.Models;

namespace RealTimeWebAnalytics.Services
{
    public class StorageService : IDisposable
    {
        private EventStoreConnection _connection;

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
            
        public void Dispose()
        {
            _connection.Close();
        }
    }
}