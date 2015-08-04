using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsArticles.Controller
{
    public class StoriesFactory
    {
        public static Model.IStories produce(string feedType, string feedIdentifier, Dictionary<string, object> conditions) {
            if (feedType.Equals("RSS")) {
                return new Model.RssStories(feedIdentifier, conditions);
            }
            else if (feedType.Equals("ATOM"))
            {
                return new Model.AtomStories(feedIdentifier, conditions);
            }
            else
            {
                throw new Exception("Cannot read content from unsupported feed type");
            }
        }
    }
}
