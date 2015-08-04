using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsArticles.View
{
    public class AtomDisplayStory : IDisplayItem
    {
        private DisplayLine Title;
        private DisplayLine Summary;
        private DisplayLine Updated;
        private DisplayLine Link;

        public AtomDisplayStory(string title, string summary, string updated, string link)
        {
            Title = new DisplayLine("TITLE", title);
            Summary = new DisplayLine("SUMMARY", summary);
            Updated = new DisplayLine("UPDATED", updated);
            Link = new DisplayLine("LINK", link);
        }
        public void draw()
        {
            Title.draw();
            Summary.draw();
            Updated.draw();
            Link.draw();
        }
    }
}
