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
    public struct RssStoryContent : IStoryContent
    {
        public RssStoryContent(string title, string description, string published, string link)
            : this()
        {
            Link = link;
            Published = published;
            Description = description;
            Title = title;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Published { get; set; }
        public string Link { get; set; }

    }
    public class RssStories : IStories
    {

        private string feedUri;
        private string storyNode;
        private string conditions;

        public RssStories(string feedIdentifier, Dictionary<string, object> pathConditions)
        {
            setFeedUri(feedIdentifier);
            storyNode = ConfigurationManager.AppSettings.Get("rssStoryNode");
            conditions = "";
            string searchCondition = "";
            string tailCondition = "";
            foreach (KeyValuePair<string, object> pair in pathConditions)
            {
                if (pair.Key.Equals("searchTerm"))
                {
                    if (pair.Value != null && !pair.Value.Equals(""))
                    {
                        searchCondition = "contains(title, '" + pair.Value + "')";
                        if (pair.Key.Equals("excludeSearchTerm"))
                        {
                            searchCondition = "not(" + searchCondition + ")";
                        }
                        searchCondition = "[" + searchCondition + "]";
                    }
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
            XmlDocument doc = new XmlDocument();
            doc.Load(feedUri);
            XmlNodeList stories = doc.SelectNodes(storyNode + conditions);
            for (int i = 0; i < stories.Count; i++)
            {
                XmlDocument item = new XmlDocument();
                item.LoadXml(stories[i].OuterXml);
                XmlNode titleNode = item.SelectSingleNode("//item/title");
                string title = titleNode.InnerText;
                XmlNode descriptionNode = item.SelectSingleNode("//item/description");
                string description = descriptionNode.InnerText;
                XmlNode linkNode = item.SelectSingleNode("//item/link");
                string link = linkNode.InnerText;
                XmlNode publishedNode = item.SelectSingleNode("//item/pubDate");
                string published = publishedNode.InnerText;
                result.Add(new RssStoryContent(title, description, published, link));
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
                var knownFeeds = ConfigurationManager.GetSection("feeds/rss") as NameValueCollection;
                if (knownFeeds != null)
                {
                    feedUri = knownFeeds.Get(feedIdentifier);
                    if (feedUri == null)
                    {
                        throw new Exception("Invalid RSS feed identifier");
                    }
                }
                else
                {
                    throw new Exception("App does not know the URI for the RSS feed for RSS feed identifier: " + feedIdentifier);
                }
            }
        }
    }
}
