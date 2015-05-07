using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RssReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var reader = new RssReader("http://rss.cnn.com/rss/edition.rss");
            reader.LoadStories();
            var stories = reader.GetTopStories();

            Searcher searcher = new Searcher(args);

            foreach (var story in stories.Where(r =>
                {
                    try
                    {
                        searcher.SearchItHard(r.Title);
                        return true;
                    }
                    catch(Exception e)
                    { return false; }
                }))
            {
                story.ToString();
            }

            Console.ReadLine();
        }
    }

    public class RssStory
    {
        public int _index { get; set; }

        public string _title;
        public string Title { get; set; }

        private string _description;
        public string Description { get; set; }

        public string _link;
        public Uri Link { get; set; }

        public DateTime Published { get; set; }

        Regex regex = new Regex("(?<scheme>[a-z]{3,5})://(?<host>[a-z0-9_-]+(.[a-z0-9_-]+)*)/(?<path>.*)");

        public RssStory(string title, string description, string link)
        {
            Title = title;
            Description = description;
            _link = link;

            //Match match = regex.Match(link);
            //Link = new UriBuilder(match.Groups["scheme"].Value, match.Groups["host"].Value, -1, match.Groups["path"].Value).Uri;

            // 10,000 should be sufficient to avoid collisions.
            _index = new Random().Next(10000);
        }

        public override string ToString()
        {
            Console.WriteLine("Title: " + Title);
            Console.WriteLine("Description: " + Description);
            Console.WriteLine("Published On: " + Published);
            Console.WriteLine("Link: " + _link);
            Console.WriteLine();

            return string.Empty;
        }
    }
}
