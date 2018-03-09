using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ContentSecurityPolicy.Models
{
    [XmlRoot(ElementName = "Source")]
    public class Source
    {
        [XmlElement(ElementName = "Allow")]
        public List<string> AllowedList { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        public bool HasAllowedList
        {
            get { return AllowedList != null && AllowedList.Any(); }
        }
    }

    [XmlRoot(ElementName = "Policy")]
    public class Policy
    {
        [XmlElement(ElementName = "Source")]
        public List<Source> Sources { get; set; }
        [XmlAttribute(AttributeName = "location")]
        public string Location { get; set; }

        public bool HasSources
        {
            get { return Sources != null && Sources.Any(); }
        }
    }

    [XmlRoot(ElementName = "ContentSecurityPolicies")]
    public class ContentSecurityPolicies
    {
        [XmlElement(ElementName = "Policy")]
        public List<Policy> Policies { get; set; }

        public bool HasPolicies
        {
            get { return Policies != null && Policies.Any(); }
        }
    }
}
