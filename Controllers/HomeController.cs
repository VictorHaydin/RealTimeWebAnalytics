using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RealTimeWebAnalytics.Services;

namespace RealTimeWebAnalytics.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var visits = await StorageService.Instance.GetAllVisits();
            var total = visits.Count();
            var stats = visits.GroupBy(v => v.CountryCode, (code, v) => new { Code = code, Count = v.Count() });
            var maxVisits = stats.Max(v => v.Count);

            ViewBag.Stats = stats.Select(s => new {s.Code, Weight = (double) s.Count/maxVisits}.ToExpando()).ToList();
            ViewBag.Visits = visits;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
