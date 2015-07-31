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
                var knownFeeds = ConfigurationManager.GetSection("feeds") as NameValueCollection;
                if (knownFeeds != null)
                {
                    feedUri = knownFeeds.Get(feedIdentifier);
                    if (feedUri == null) {
                        feedUri = feedIdentifier;
                    }
                }
            }
        }
    }
}
