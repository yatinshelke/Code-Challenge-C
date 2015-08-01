using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.XPath;

namespace NewsStories
{
    namespace Model
    {
        public struct StoryContent
        {
            public StoryContent(string title, string description, string published, string link) : this()
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
        public class Stories
        {

            private string feedUri;
            private string xpath;

            public Stories(string feedIdentifier, string conditions)
            {
                setFeedUri(feedIdentifier);
                xpath = conditions;
            }

            public void init()
            {

            }
            public List<StoryContent> read()
            {
                List<StoryContent> result = new List<StoryContent>();
                XmlDocument doc = new XmlDocument();
                doc.Load(feedUri);
                XmlNodeList stories = doc.SelectNodes(xpath);
                for (int i = 0; i < stories.Count; i++) {
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
                    result.Add(new StoryContent(title, description, published, link));
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
                    var knownFeeds = ConfigurationManager.GetSection("feeds") as NameValueCollection;
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
}
