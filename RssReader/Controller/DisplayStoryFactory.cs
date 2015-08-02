using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsArticles.Controller
{
    public class DisplayStoryFactory
    {
        public static View.DisplayStory produce(Model.IStoryContent storyContent)
        {
            if (storyContent.GetType().Equals(typeof(Model.RssStoryContent))) {
                Model.RssStoryContent rssStoryContent = (Model.RssStoryContent)storyContent;
                return new View.DisplayStory(rssStoryContent.Title, rssStoryContent.Description, rssStoryContent.Published, rssStoryContent.Link);
            }
            else
            {
                return null;
            }
        }
    }
}
