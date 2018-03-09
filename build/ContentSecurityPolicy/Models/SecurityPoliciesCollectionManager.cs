using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;

namespace ContentSecurityPolicy.Models
{
    class SecurityPoliciesCollectionManager
    {
        private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(ContentSecurityPolicies));
        private static readonly string FullPath = "~/Config/ContentSecurityPolicies.config";
        private static readonly CacheDependency ConfigDocDependency = new CacheDependency(HttpContext.Current.Server.MapPath(FullPath));

        public static ContentSecurityPolicies GetPolicies()
        {
            ContentSecurityPolicies file = new ContentSecurityPolicies();

            var cspsConfig = (ContentSecurityPolicies)HttpContext.Current.Cache["ContentSecurityPolicyConfig"];

            if (cspsConfig != null)
            {
                return cspsConfig;
            }

            // Create a StreamReader
            if (File.Exists(HttpContext.Current.Server.MapPath(FullPath)))
            {
                using (TextReader reader = new StreamReader(HttpContext.Current.Server.MapPath(FullPath)))
                {
                    file = (ContentSecurityPolicies)XmlSerializer.Deserialize(reader);
                }
            }

            HttpContext.Current.Cache.Insert("ContentSecurityPolicyConfig", file, ConfigDocDependency, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.High, null);

            // Return the object
            return file;
        }
    }
}
