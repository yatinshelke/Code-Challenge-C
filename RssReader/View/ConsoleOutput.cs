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
    public class ConsoleOutput
    {
        private List<IDisplayItem> DisplayStories;
        public ConsoleOutput(List<IDisplayItem> displayStories)
        {
            DisplayStories = displayStories;
        }
        public void draw()
        {
            foreach (IDisplayItem displayStory in DisplayStories) {
                displayStory.draw();
                Console.Out.WriteLine("-------------------------------------------");
                Console.Out.WriteLine("");
            }
        }
    }
}
