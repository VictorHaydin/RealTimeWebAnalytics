using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using RealTimeWebAnalytics.Models;
using RealTimeWebAnalytics.Services;

namespace RealTimeWebAnalytics.Controllers
{
    public class CollectorController : AsyncController
    {
        public async Task<ActionResult> Collect()
        {
            var visit = new Visit {IP = IPAddress.Parse(Request.UserHostAddress).MapToIPv4(), Timestamp = DateTime.UtcNow};
            await GeoLocator.Instance.Locate(visit);
            await StorageService.Instance.StoreVisit(visit);
            return Content("OK");
        }
    }
}
