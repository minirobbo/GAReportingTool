using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Web;

namespace GAReporting.Web.BL.Analytics
{
    public class AuthenticationHelper
    {/// <summary>
     /// Authenticate to Google Using Oauth2
     /// Documentation https://developers.google.com/accounts/docs/OAuth2
     /// </summary>
     /// <param name="clientId">From Google Developer console https://console.developers.google.com</param>
     /// <param name="clientSecret">From Google Developer console https://console.developers.google.com</param>
     /// <param name="userName">A string used to identify a user.</param>
     /// <returns></returns>
        public static AnalyticsService AuthenticateOauth(string clientId, string clientSecret, string userName)
        {

            string[] scopes = new string[] { AnalyticsService.Scope.Analytics,  // view and manage your analytics data
                                             AnalyticsService.Scope.AnalyticsEdit,  // edit management actives
                                             AnalyticsService.Scope.AnalyticsManageUsers,   // manage users
                                             AnalyticsService.Scope.AnalyticsReadonly};     // View analytics data

            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore("Comprend.GoogleAnalytics.Auth.Store")).Result;

                AnalyticsService service = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Analytics API Sample",
                });
                return service;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }

        }

        /// <summary>
        /// Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>
        /// <param name="serviceAccountEmail">From Google Developer console https://console.developers.google.com</param>
        /// <param name="keyFilePath">Location of the Service account key file downloaded from Google Developer console https://console.developers.google.com</param>
        /// <returns></returns>
        public static AnalyticsService AuthenticateServiceAccount(string serviceAccountEmail, string keyFilePath)
        {

            // check the file exists
            if (!File.Exists(keyFilePath))
            {
                Console.WriteLine("An Error occurred - Key file does not exist");
                return null;
            }

            string[] scopes = new string[] { AnalyticsService.Scope.Analytics,  // view and manage your analytics data
                                             AnalyticsService.Scope.AnalyticsEdit,  // edit management actives
                                             AnalyticsService.Scope.AnalyticsManageUsers,   // manage users
                                             AnalyticsService.Scope.AnalyticsReadonly};     // View analytics data            

            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));

                // Create the service.
                AnalyticsService service = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Analytics API Sample",
                });
                return service;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }
        }



        /// <summary>
        /// Not working: Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>
        /// <param name="serviceAccountEmail">From Google Developer console https://console.developers.google.com</param>
        /// <param name="keyFilePath">Location of the Service account key file downloaded from Google Developer console https://console.developers.google.com</param>
        /// <returns></returns>
        public static AnalyticsService AuthenticateServiceAccountJson(string keyFilePath)
        {
            // check the file exists
            if (!File.Exists(keyFilePath))
            {
                Console.WriteLine("An Error occurred - Key file does not exist");
                return null;
            }
            UserCredential OAuthcredential;
            try
            {
                using (var stream = new FileStream(keyFilePath, FileMode.Open, FileAccess.Read))
                {
                    OAuthcredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        new[] { AnalyticsService.Scope.AnalyticsReadonly },
                        "user", CancellationToken.None, new FileDataStore("Analytics.Auth.Store")).Result;
                }

                //we need to create AnalyticsService class object to use GA api
                AnalyticsService service = new AnalyticsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = OAuthcredential,
                    ApplicationName = "Analytics API sample",
                });

                return service;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }

            
        }

        internal static AnalyticsService GetSetSession()
        {

            AnalyticsService service;

            if (HttpContext.Current.Session["AnalyticsServiceAccount"] != null)
            {
                return HttpContext.Current.Session["AnalyticsServiceAccount"] as AnalyticsService;
            }
            else
            {
                
                string SERVICE_ACCOUNT_EMAIL = "comprend@comprend-gareportingtool.iam.gserviceaccount.com";
                string SERVICE_ACCOUNT_KEYFILE = HttpContext.Current.Server.MapPath("/ComprendGAReportingTool.p12");
                service = AuthenticateServiceAccount(SERVICE_ACCOUNT_EMAIL, SERVICE_ACCOUNT_KEYFILE);
                HttpContext.Current.Session["AnalyticsServiceAccount"] = service;

                return service;
            }

        }
    }
}