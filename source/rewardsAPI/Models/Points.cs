using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RewardsAPI.Models
{
    public class Points
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public double Point { get; set; }
        public string Desc { get; set; }
        public string image { get; set; }
        public DateTime sDate { get; set; }
        public DateTime eDate { get; set; }
        public string Link { get; set; }
        public string LinkLabel { get; set; }
        public string printmsg { get; set; }
        public string textstring { get; set; }
    }

    public class UserPoints
    {
        public string Name { get; set; }
        //public string Code { get; set; }
        public double Point { get; set; }
        public string Description { get; set; }
        public string image { get; set; }
        public string Link { get; set; }
        //public string LinkLabel { get; set; }
        //public string Point { get; set; }
        //public string textstring { get; set; }
    }

    public class YTD {
        public string ytd { get; set; }
    }
}