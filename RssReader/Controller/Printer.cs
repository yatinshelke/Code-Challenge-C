using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public Printer(View.UserInput input) : this()
        {
            if (input.Options["feedIdentifier"] != null) {
                feedIdentifier = input.Options["feedIdentifier"];
            }
            if (input.Options["/with"] != null) {
                searchStringInArticle = input.Options["/with"];
                excludeArticleWithSearchString = false;
            } 
            else if (input.Options["/without"] != null) {
                searchStringInArticle = input.Options["/without"];
                excludeArticleWithSearchString = true;
            }
            if (input.Options["/latest"] != null) {
                maxStoriesCount = Int32.Parse(input.Options["/latest"]);
            }
        }

        public void print()
        {

        }

        private void createXpath() {

        }
    }
}
