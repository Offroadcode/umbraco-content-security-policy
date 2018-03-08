using System;
using System.Web;
using System.Xml;
using ContentSecurityPolicy.Helpers;

namespace ContentSecurityPolicy.HttpModules
{
    public class ContentSecurityPolicyModule : IHttpModule
    {
        private readonly string FullPath = "~/Config/ContentSecurityPolicies.config";

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

                XmlDocument document = new XmlDocument();

                document.Load(HttpContext.Current.Server.MapPath(FullPath));
                var policies = document.SelectNodes("//Policy");

                if (policies != null)
                {
                    foreach (XmlNode policy in policies)
                    {
                        var policyHeader = HttpHelpers.BuildContentSecurityPolicyHeader(policy);

                        if (!string.IsNullOrEmpty(response.Headers["Content-Security-Policy"]))
                        {
                            response.Headers.Remove("Content-Security-Policy");
                        }

                        if (policy.Attributes != null && policy.Attributes["location"] != null && !string.IsNullOrEmpty(policy.Attributes["location"].Value))
                        {
                            var location = policy.Attributes["location"].Value;
                            if (location.StartsWith("/"))
                            {
                                if (HttpHelpers.IsFolderRequest(location))
                                {
                                    response.AddHeader("Content-Security-Policy", policyHeader);
                                }
                            }
                            else if (location.StartsWith("http://") || location.StartsWith("https://"))
                            {
                                if (HttpHelpers.IsDomainRequest(location))
                                {
                                    response.AddHeader("Content-Security-Policy", policyHeader);
                                }
                            }
                        }
                        else
                        {
                            response.AddHeader("Content-Security-Policy", policyHeader);
                        }
                    }
                }
                
                    //response.AddHeader("Content-Security-Policy",
                    //    "default-src 'self' data:; style-src 'self' 'unsafe-inline' fonts.googleapis.com; script-src 'self' 'unsafe-inline' 'unsafe-eval' code.jquery.com ajax.aspnetcdn.com; font-src 'self' fonts.gstatic.com");

                    //response.AddHeader("Content-Security-Policy", "default-src 'self' data:; style-src 'self' 'unsafe-inline' fonts.googleapis.com; script-src 'self' 'unsafe-inline' code.jquery.com ajax.aspnetcdn.com; font-src 'self' fonts.gstatic.com");
            }
        }
    }
}
