using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsArticles.View
{
    public class RssDisplayStory : IDisplayItem
    {
        private DisplayLine Title;
        private DisplayLine Description;
        private DisplayLine Published;
        private DisplayLine Link;

        public RssDisplayStory(string title, string description, string published, string link)
        {
            Title = new DisplayLine("TITLE", title);
            Description = new DisplayLine("DESCRIPTION", description);
            Published = new DisplayLine("PUBLISHED ON", published);
            Link = new DisplayLine("LINK", link);
        }
        public void draw()
        {
            Title.draw();
            Description.draw();
            Published.draw();
            Link.draw();
        }
    }
}
