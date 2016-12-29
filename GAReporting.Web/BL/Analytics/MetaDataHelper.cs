using System.Linq;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;

namespace GAReporting.Web.BL.Analytics
{
    public class MetaDataHelper
    {
        /// <summary>
        /// Returns full list from the MetaData API
        /// Documentation: https://developers.google.com/analytics/devguides/reporting/metadata/v3/reference/metadata/columns/list
        /// </summary>
        /// <param name="service">Valid Analytics Service</param>
        /// <returns></returns>
        public static Columns MetaDataList(AnalyticsService service)
        {

            return service.Metadata.Columns.List("ga").Execute();
        }

        /// <summary>
        /// Returns only the Dimensions From the MetaData API
        /// Documentation: https://developers.google.com/analytics/devguides/reporting/metadata/v3/reference/metadata/columns/list
        /// </summary>
        /// <param name="service">Valid Analytics Service</param>
        /// <returns></returns>
        public static Columns GetDimensions(AnalyticsService service)
        {
            Columns result = MetaDataHelper.MetaDataList(service);
            result.Items = result.Items.Where(a => a.Attributes["type"] == "DIMENSION").ToList();
            result.TotalResults = result.Items.Count();

            return result;

        }

        /// <summary>
        /// Returns only the Metrics from the MetaData api
        /// Documentation: https://developers.google.com/analytics/devguides/reporting/metadata/v3/reference/metadata/columns/list
        /// </summary>
        /// <param name="service">Valid Analytics Service</param>
        /// <returns></returns>
        public static Columns GetMetrics(AnalyticsService service)
        {
            Columns result = MetaDataHelper.MetaDataList(service);
            result.Items = result.Items.Where(a => a.Attributes["type"] == "METRIC").ToList();
            result.TotalResults = result.Items.Count();

            return result;

        }
        /// <summary>
        /// Lets you search on the different attributes returned with an item from the metadata api.
        /// Documentation: https://developers.google.com/analytics/devguides/reporting/metadata/v3/reference/metadata/columns/list
        /// </summary>
        /// <param name="service">Valid Analytics Service</param>
        /// <returns></returns>
        public static Columns GetSearchattributes(AnalyticsService service, string attributeName, string attributeValue)
        {
            Columns result = MetaDataHelper.MetaDataList(service);
            result.Items = result.Items.Where(a => a.Attributes[attributeName] == attributeValue).ToList();
            result.TotalResults = result.Items.Count();

            return result;

        }
    }
}