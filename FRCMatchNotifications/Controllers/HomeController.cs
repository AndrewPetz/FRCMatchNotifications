using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FRCMatchNotifications.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string connectionString = "";
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("FRCNotifications.dbo.usp_GetNumbersForTeams", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add(new SqlParameter("@team1", "frc7021"));
                    cmd.Parameters.Add(new SqlParameter("@team2", "Test"));
                    cmd.Parameters.Add(new SqlParameter("@team3", "Test"));
                    cmd.Parameters.Add(new SqlParameter("@team4", "Test"));
                    cmd.Parameters.Add(new SqlParameter("@team5", "Test"));
                    cmd.Parameters.Add(new SqlParameter("@team6", "Test"));

                    using(SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                         if(dr.HasRows)
                        {
                            while(dr.Read())
                            {
                                var returned = dr.GetValue(0);
                                Console.WriteLine(returned);
                            }
                        }
                    }

                    cmd.Dispose();
                }
            } catch(Exception e)
            {
                throw e;
            }
            
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