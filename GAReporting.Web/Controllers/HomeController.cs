using GAReporting.Web.BL;
using GAReporting.Web.BL.Analytics;
using Google.Apis.Analytics.v3.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GAReporting.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new Models.Analytics.AccountList()
            {
                Name = "Hello to Jason Isaacs"
            };

            var service = AuthenticationHelper.GetSetSession();
            if (service != null)
            {
                model.AccountSummaries = ManagmentHelper.AccountSummaryList(service).Items; 
            }
            return View(model);
        }

        //public ActionResult Report(string id, string pid)
        //{





        //    var service = AuthenticationHelper.GetSetSession();
        //    ReportingHelper.OptionalValues options = new ReportingHelper.OptionalValues();
        //    options.Dimensions = "ga:date"; 
        //    GaData bob = ReportingHelper.Get(service, pid, "30daysAgo", "today", "ga:sessions", options);
            

        //    var model = new Models.Analytics.Report()
        //    {
        //        Name = "Your report for " + id,
        //        GaDataUsers = bob
        //    };


        //    //ViewBag.Title = id;

        //    //ViewBag.Message = "Your report page." + pid;

        //    return View(model);
        //}

        //public ActionResult About()
        //{

        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}


    }
}