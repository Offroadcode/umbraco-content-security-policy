using System.Web;

namespace ContentSecurityPolicy.Helpers
{
    public static class HttpHelpers
    {
        public static bool IsBackOfficeRequest()
        {
            var inBackOffice = false;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.Path.Contains("/umbraco/"))
                {
                    inBackOffice = true;
                }
                else if (HttpContext.Current.Request.UrlReferrer != null &&
                         HttpContext.Current.Request.UrlReferrer.PathAndQuery.Contains("/umbraco/"))
                {
                    inBackOffice = true;
                }
            }

            return inBackOffice;
        }
    }
}
