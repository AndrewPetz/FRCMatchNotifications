using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FRCMatchNotifications.Helpers;

namespace FRCMatchNotifications.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<string> test = new List<string>();
            test = DataAccess.GetNumbersForTeams("frc7021");
            int test2 = DataAccess.InsertNumberForTeam("5555555555", "frc7021");

            Console.WriteLine(test);
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}