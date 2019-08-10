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
        [HttpGet]
        public ActionResult Index()
        {
            return View(new Models.HomeModel("",""));
        }

        [HttpPost]
        public ActionResult Index(string Number, string Team)
        {
            Models.HomeModel model = new Models.HomeModel(Number, Team);
            try
            {
                DataAccess.InsertNumberForTeam(model.Number, model.Team);
            } catch(Exception e)
            {
                throw e;
            }
            return View(model);
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