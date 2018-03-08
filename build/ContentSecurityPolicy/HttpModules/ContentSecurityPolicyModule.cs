using System;
using System.Web;
using ContentSecurityPolicy.Helpers;

namespace ContentSecurityPolicy.HttpModules
{
    public class ContentSecurityPolicyModule : IHttpModule
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
            // make sure you don't write a memory leak!
            // dispose of everything that needs it!
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        private void BeginRequest(object sender, EventArgs e)
        {
            // Make sure there isn't a content security policy already set. We're not going to override an existing one.
            if (string.IsNullOrEmpty(HttpContext.Current.Response.Headers["Content-Security-Policy"]))
            {
                var response = HttpContext.Current.Response;

                if (HttpHelpers.IsBackOfficeRequest())
                {
                    response.AddHeader("Content-Security-Policy",
                        "default-src 'self' data:; style-src 'self' 'unsafe-inline' fonts.googleapis.com; script-src 'self' 'unsafe-inline' 'unsafe-eval' code.jquery.com ajax.aspnetcdn.com; font-src 'self' fonts.gstatic.com");
                }
                else
                {
                    response.AddHeader("Content-Security-Policy", "default-src 'self' data:; style-src 'self' 'unsafe-inline' fonts.googleapis.com; script-src 'self' 'unsafe-inline' code.jquery.com ajax.aspnetcdn.com; font-src 'self' fonts.gstatic.com");
                }
            }
        }
    }
}
