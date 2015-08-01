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
        public Printer(View.UserInput input) : this()
        {
            // Always a dilemma how much validation to do and where
            // For now expecting View to validate user input
            // TODO: Clear definition of protocol for each of M, V and C
            //       so it is clear what validation to do and where.
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
            Model.RssStories stories = new Model.RssStories(feedIdentifier, createXpath());
            List<Model.RssStoryContent> storiesContent = stories.read();
            List<View.DisplayStory> displayStories = new List<View.DisplayStory>();
            foreach (var storyContent in storiesContent) {
                View.DisplayStory displayStory = new View.DisplayStory(storyContent.Title, storyContent.Description, storyContent.Published, storyContent.Link);
                displayStories.Add(displayStory);
            }
            View.ConsoleOutput consoleOutput = new View.ConsoleOutput(displayStories);
            consoleOutput.draw();
        }

        private string createXpath() 
        {
            string xpath = "//channel/item";
            if (searchStringInArticle != null)
            {
                string searchComponent = "contains(title, '" + searchStringInArticle + "')";
                if (excludeArticleWithSearchString) {
                    searchComponent = "not(" + searchComponent + ")";
                }
                searchComponent = "[" + searchComponent + "]";
                xpath += searchComponent;
            }
            xpath += "[position()>last()-" + maxStoriesCount + "]";
            return xpath;
        }
    }
}
