using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace RssReader
{
    namespace Model
    {
        public class Stories
        {
            private string feedUri;
            public Stories(string feedIdentifier)
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
