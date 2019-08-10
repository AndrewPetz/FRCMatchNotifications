using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FRCMatchNotifications.Models
{
    public class HomeModel
    {
        public string Number { get; set; }
        public string Team { get; set; }

        public HomeModel(string number, string team)
        {
            this.Number = number;
            this.Team = team;
        }
    }
}