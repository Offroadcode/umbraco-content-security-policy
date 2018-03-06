using System.Web;
using System.Web.Mvc;

namespace ContentSecurityPolicy.Extensions
{
    public static class SecurityExtensions
    {
        public static void GenerateContentSecurityPolicyHeader(this HtmlHelper helper)
        {
            var response = HttpContext.Current.Response;

            response.AddHeader("Content-Security-Policy", "default-src 'self';");
        }
    }
}
