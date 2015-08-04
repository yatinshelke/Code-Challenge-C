using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.XPath;

namespace NewsArticles.Model
{
    public struct AtomStoryContent : IStoryContent
    {
        public AtomStoryContent(string title, string summary, string updated, string link)
            : this()
        {
            Link = link;
            Updated = updated;
            Summary = summary;
            Title = title;
        }

        public string Title { get; set; }
        public string Summary { get; set; }
       public string Updated { get; set; }
        public string Link { get; set; }

    }
    public class AtomStories : IStories
    {

        private string feedUri;
        private string nsPrefix;
        private string storyNode;
        private string conditions;

        public AtomStories(string feedIdentifier, Dictionary<string, object> pathConditions)
        {
            setFeedUri(feedIdentifier);
            nsPrefix = ConfigurationManager.AppSettings.Get("atomNSPrefix");
            storyNode = "//" + nsPrefix + ":" + ConfigurationManager.AppSettings.Get("atomStoryNodeName");
            conditions = "";
            string searchCondition = "";
            string tailCondition = "";
            foreach (KeyValuePair<string, object> pair in pathConditions)
            {
                if (pair.Key.Equals("searchTerm"))
                {
                    searchCondition = "contains(" + nsPrefix + ":" + "title, '" + pair.Value + "')";
                    if (pair.Key.Equals("excludeSearchTerm"))
                    {
                        searchCondition = "not(" + searchCondition + ")";
                    }
                    searchCondition = "[" + searchCondition + "]";
                }
                else if (pair.Key.Equals("tailSize"))
                {
                    tailCondition = "[position()>last()-" + pair.Value + "]";
                }
            }
            conditions = searchCondition + tailCondition;
        }

        public void init()
        {

        }
        public List<IStoryContent> read()
        {
            List<IStoryContent> result = new List<IStoryContent>();
            XPathDocument xDoc = new XPathDocument(feedUri);
            XPathNavigator xNav = xDoc.CreateNavigator();
            XmlNamespaceManager ns = new XmlNamespaceManager(xNav.NameTable);
            ns.AddNamespace(nsPrefix, "http://www.w3.org/2005/Atom");
            XPathNodeIterator xIter = xNav.Select(storyNode + conditions, ns);
            while (xIter.MoveNext())
            {
                string x = xIter.Current.OuterXml;
                XmlDocument item = new XmlDocument();
                item.LoadXml(x);
                string nodePrefix = storyNode + "/" + nsPrefix + ":";
                XmlNode titleNode = item.SelectSingleNode(nodePrefix + "title", ns);
                string title = titleNode.InnerText;
                XmlNode summaryNode = item.SelectSingleNode(nodePrefix + "summary", ns);
                string summary = summaryNode.InnerText;
                XmlNode linkNode = item.SelectSingleNode(nodePrefix + "link", ns);
                string link = linkNode.InnerText;
                XmlNode updatedNode = item.SelectSingleNode(nodePrefix + "updated", ns);
                string updated = updatedNode.InnerText;
                result.Add(new AtomStoryContent(title, summary, updated, link));
            }
            return result;
        }

        private void setFeedUri(string feedIdentifier)
        {
            Uri uriResult;
            bool feedIdentifierIsUri = false;
            if (Uri.TryCreate(feedIdentifier, UriKind.Absolute, out uriResult))
            {
                if (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                {
                    feedIdentifierIsUri = true;
                }
            }
            if (feedIdentifierIsUri)
            {
                feedUri = feedIdentifier;
            }
            else
            {
                var knownFeeds = ConfigurationManager.GetSection("feeds/atom") as NameValueCollection;
                if (knownFeeds != null)
                {
                    feedUri = knownFeeds.Get(feedIdentifier);
                    if (feedUri == null)
                    {
                        throw new Exception("Invalid ATOM feed identifier");
                    }
                }
                else
                {
                    throw new Exception("App does not know the URI for the ATOM feed for RSS feed identifier: " + feedIdentifier);
                }
            }
        }
    }
}
