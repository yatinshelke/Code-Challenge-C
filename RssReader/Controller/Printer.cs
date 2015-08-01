using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NewsArticles.Controller
{
    public class Printer
    {
        public string feedIdentifier { private get; set; }
        public string searchStringInArticle { private get; set; }
        public bool excludeArticleWithSearchString { private get; set; }
        public int maxStoriesCount { private get; set; }
        public Printer()
        {
            feedIdentifier = "BBC";
            searchStringInArticle = null;
            excludeArticleWithSearchString = true;
            maxStoriesCount = 5;
        }
        public Printer(string feedNameOrUri, string searchString, bool excludeArticle, int maxCount) : this()
        {
            feedIdentifier = feedNameOrUri;
            searchStringInArticle = searchString;
            excludeArticleWithSearchString = excludeArticle;
            maxStoriesCount = maxCount;
        }

        public void print()
        {

        }

        private void createXpath() {

        }
    }
}
