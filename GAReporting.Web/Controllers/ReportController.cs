using GAReporting.Web.BL.Analytics;
using System.Web.Mvc;
using Google.Apis.Analytics.v3.Data;

namespace GAReporting.Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard(string accountId, string webPropertyId, string profileId)
        {
            return ViewReport(accountId, webPropertyId, profileId, "Dashboard");
        }

        public ActionResult Realtime(string accountId, string webPropertyId, string profileId)
        {
            return ViewReport(accountId, webPropertyId, profileId, "Realtime");
        }


        public ActionResult ViewReport(string accountId, string webPropertyId, string profileId, string viewName = "ViewReport")
        {
            var service = AuthenticationHelper.GetSetSession();
            Webproperty account = ManagmentHelper.WebpropertyGet(service, accountId, webPropertyId);


            string intro = account.WebsiteUrl;

            var model = new Models.Analytics.Report()
            {
                Title = "Your report for " + account.Name,
                Webproperty = account,
                Intro = intro,
                AccountId = accountId,
                WebPropertyId = webPropertyId,
                ProfileId = profileId
            };

            return View(viewName, model);
        }





        public PartialViewResult RealTimeMetric(string profileId, string metric, string dimensions = null, string sortDimensions = null, string viewname = "RealTimeMetric")
        {
            var service = AuthenticationHelper.GetSetSession();


            RealTimeHelper.OptionalValues rtOptions = new RealTimeHelper.OptionalValues();
            if (!string.IsNullOrWhiteSpace(dimensions))
            {
                rtOptions.Dimensions = dimensions;
            }
            if (!string.IsNullOrWhiteSpace(sortDimensions))
            {
                rtOptions.Sort = sortDimensions;

            }
            //Make sure the profile id you send is valid.  
           RealtimeData realTimeData = RealTimeHelper.Get(service, profileId, metric, rtOptions);


            if (realTimeData != null)
            {

                return PartialView("~/Views/Partials/Report/"+ viewname + ".cshtml", realTimeData);
            }
            else {
                return null;
            }
        }



        public PartialViewResult ReportingMetric(string profileId, string startDate = "10daysAgo", string endDate = "today", 
            string metric = "ga:sessions", string dimensions = null, 
            string sortDimensions = null, string viewname = "ReportingMetric", int maxResults = 20 )
        {
            var service = AuthenticationHelper.GetSetSession();


            ReportingHelper.OptionalValues rtOptions = new ReportingHelper.OptionalValues();
            if (!string.IsNullOrWhiteSpace(dimensions))
            {
                rtOptions.Dimensions = dimensions;
            }

            if (!string.IsNullOrWhiteSpace(sortDimensions))
            {
                rtOptions.Sort = sortDimensions;
            }

            rtOptions.MaxResults = maxResults;
            //rtOptions.Sampling = Google.Apis.Analytics.v3.DataResource.GaResource.GetRequest.SamplingLevelEnum.FASTER;

            //Make sure the profile id you send is valid.  
            GaData gaData = ReportingHelper.Get(service, profileId, startDate, endDate, metric, rtOptions);


            var data = ReportingV4Helper.Get(service as Google.Apis.AnalyticsReporting.v4.AnalyticsReportingService, profileId, startDate, endDate, metric, dimensions);

            if (gaData != null)
            {

                return PartialView("~/Views/Partials/Report/" + viewname + ".cshtml", gaData);
            }
            else
            {
                return null;
            }
        }

    }
}