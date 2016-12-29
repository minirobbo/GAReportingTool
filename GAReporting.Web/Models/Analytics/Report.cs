using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Analytics.v3.Data;

namespace GAReporting.Web.Models.Analytics
{
    public class Report
    {
        public string Title { get; set; }
        public string Intro { get; set; }
        public string AccountId { get; set; }
        public string PropertyId { get; set; }
        public Webproperty Webproperty { get; internal set; }
        public string WebPropertyId { get; internal set; }
        public string ProfileId { get; internal set; }


        //public GaData GaDataUsers { get; set; }
    }
}