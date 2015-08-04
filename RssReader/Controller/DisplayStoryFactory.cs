using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsArticles.Controller
{
    public class DisplayStoryFactory
    {
        public static View.IDisplayItem produce(Model.IStoryContent storyContent)
        {
            if (storyContent.GetType().Equals(typeof(Model.RssStoryContent))) {
                Model.RssStoryContent rssStoryContent = (Model.RssStoryContent)storyContent;
                return new View.RssDisplayStory(rssStoryContent.Title, rssStoryContent.Description, rssStoryContent.Published, rssStoryContent.Link);
            }
            else if (storyContent.GetType().Equals(typeof(Model.AtomStoryContent))) {
                Model.AtomStoryContent atomStoryContent = (Model.AtomStoryContent)storyContent;
                return new View.AtomDisplayStory(atomStoryContent.Title, atomStoryContent.Summary, atomStoryContent.Updated, atomStoryContent.Link);
            }
            else
            {
                throw new Exception("Cannot display unsupported news content");
            }
        }
    }
}
