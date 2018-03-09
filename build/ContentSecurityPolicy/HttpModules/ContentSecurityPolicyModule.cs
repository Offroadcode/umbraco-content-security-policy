using System;
using System.Web;
using System.Web.Caching;
using System.Xml;
using ContentSecurityPolicy.Helpers;
using ContentSecurityPolicy.Models;

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

                var csps = SecurityPoliciesCollectionManager.GetPolicies();

                if (csps != null && csps.HasPolicies)
                {
                    foreach (var policy in csps.Policies)
                    {
                        var policyHeader = HttpHelpers.BuildContentSecurityPolicyHeader(policy);

                        if (!string.IsNullOrEmpty(policy.Location))
                        {
                            if (policy.Location.StartsWith("/"))
                            {
                                if (HttpHelpers.IsFolderRequest(policy.Location))
                                {
                                    if (!string.IsNullOrEmpty(response.Headers["Content-Security-Policy"]))
                                    {
                                        response.Headers.Remove("Content-Security-Policy");
                                    }

                                    response.AddHeader("Content-Security-Policy", policyHeader);
                                }
                            }
                            else if (policy.Location.StartsWith("http://") || policy.Location.StartsWith("https://"))
                            {
                                if (HttpHelpers.IsDomainRequest(policy.Location))
                                {
                                    if (!string.IsNullOrEmpty(response.Headers["Content-Security-Policy"]))
                                    {
                                        response.Headers.Remove("Content-Security-Policy");
                                    }

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
            }
        }
    }
}
