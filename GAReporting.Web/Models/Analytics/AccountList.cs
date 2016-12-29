using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Analytics.v3.Data;

namespace GAReporting.Web.Models.Analytics
{
    public class AccountList
    {
        public string Name { get; set; }

        public IEnumerable<AccountSummary> AccountSummaries { get; set; }
    }
}