using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ContentSecurityPolicy.Models;

namespace ContentSecurityPolicy.Helpers
{
    public static class HttpHelpers
    {
        public static bool IsFolderRequest(string folder)
        {
            var inFolder = false;
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.Path.Contains(folder))
                {
                    inFolder = true;
                }
                else if (HttpContext.Current.Request.UrlReferrer != null && HttpContext.Current.Request.UrlReferrer.PathAndQuery.Contains(folder))
                {
                    inFolder = true;
                }
            }

            return inFolder;
        }

        public static bool IsDomainRequest(string domain)
        {
            var inDomain = false;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request.RawUrl.StartsWith(domain))
                {
                    inDomain = true;
                }
                else if (HttpContext.Current.Request.UrlReferrer != null && HttpContext.Current.Request.Url.AbsolutePath.StartsWith(domain))
                {
                    inDomain = true;
                }
            }

            return inDomain;
        }

        public static string BuildContentSecurityPolicyHeader(Policy policy)
        {
            var header = string.Empty;

            if (policy.HasSources)
            {
                header = CombineAllowedSources(policy.Sources);
            }

            return header;
        }

        public static string CombineAllowedSources(IEnumerable<Source> sources)
        {
            var sourceList = new List<string>();
            foreach (var source in sources)
            {
                if (!string.IsNullOrEmpty(source.Name))
                {
                    var allowedList = new List<string>();
                    if (source.HasAllowedList)
                    {
                        foreach (var allowedSource in source.AllowedList)
                        {
                            allowedList.Add(allowedSource);
                        }
                    }

                    var sourceString = string.Join(" ", allowedList);
                    var sb = new StringBuilder();
                    sb.Append(source.Name + " ");
                    sb.Append(sourceString);
                    sb.Append(";");
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        sourceList.Add(sb.ToString());
                    }
                }
            }

            if (sourceList.Any())
            {
                return string.Join(" ", sourceList);
            }

            return string.Empty;
        }
    }
}
