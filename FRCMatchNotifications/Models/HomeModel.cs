using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FRCMatchNotifications.Models
{
    public class HomeModel
    {
        public string number { get; set; }
        public string team { get; set; }

        public HomeModel(string number, string team)
        {
            this.number = number;
            this.team = team;
        }
    }
}