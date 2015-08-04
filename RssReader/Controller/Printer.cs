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
        public string feedType { private get; set; }
        public Dictionary<string, object> conditions { private get; set; }
        public Printer()
        {
            feedIdentifier = "BBC";
            feedType = "RSS";
            conditions = new Dictionary<string, object>();
            conditions["searchTerm"] = null;
            conditions["excludeSearchTerm"] = true;
            conditions["tailSize"] = 5;
        }
        public Printer(View.UserInput input) : this()
        {
            // Always a dilemma how much validation to do and where
            // For now expecting View to validate user input
            // TODO: Clear definition of protocol for each of M, V and C
            //       so it is clear what validation to do and where.
            if (input.Options["feedType"] != null)
            {
                feedType = input.Options["feedType"];
            }
            if (input.Options["feedIdentifier"] != null)
            {
                feedIdentifier = input.Options["feedIdentifier"];
            }
            if (input.Options["/with"] != null) {
                conditions["searchTerm"] = input.Options["/with"];
                conditions["excludeSearchTerm"] = false;
            } 
            else if (input.Options["/without"] != null) {
                conditions["searchTerm"] = input.Options["/with"];
                conditions["excludeSearchTerm"] = true;
            }
            if (input.Options["/latest"] != null) {
                conditions["tailSize"] = input.Options["/latest"];
            }
        }

        public void print()
        {
            Model.IStories stories = StoriesFactory.produce(feedType, feedIdentifier, conditions);
            List<Model.IStoryContent> storiesContent = stories.read();
            List<View.IDisplayItem> displayStories = new List<View.IDisplayItem>();
            foreach (var storyContent in storiesContent) {
                View.IDisplayItem displayStory = Controller.DisplayStoryFactory.produce(storyContent);
                displayStories.Add(displayStory);
            }
            View.ConsoleOutput consoleOutput = new View.ConsoleOutput(displayStories);
            consoleOutput.draw();
        }
    }
}
