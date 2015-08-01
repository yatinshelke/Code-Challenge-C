using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsArticles.View
{
    public class DisplayLine : IDisplayItem
    {
        private string Label;
        private string Value;

        public DisplayLine(string label, string value)
        {
            Label = label;
            Value = value;
        }

        public void draw()
        {
            Console.Out.WriteLine(Label + ": " + Value);
        }
    }
    public class DisplayStory : IDisplayItem
    {
        private DisplayLine Title;
        private DisplayLine Description;
        private DisplayLine Published;
        private DisplayLine Link;

        public DisplayStory(string title, string description, string published, string link)
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
    public class ConsoleOutput
    {
        private List<DisplayStory> DisplayStories;
        public ConsoleOutput(List<DisplayStory> displayStories)
        {
            DisplayStories = displayStories;
        }
        public void draw()
        {
            foreach (DisplayStory displayStory in DisplayStories) {
                displayStory.draw();
                Console.Out.WriteLine("-------------------------------------------");
                Console.Out.WriteLine("");
            }
        }
    }
}
