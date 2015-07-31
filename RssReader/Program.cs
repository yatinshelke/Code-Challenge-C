using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

// *** Stating overall requirements
// ***   1. print latest 5 articles based on conditions stated in arguments to program
// ***   2. Call program with max 2 command line args
// ***   3. if only arg1: print latest 5 articles whose title contain arg1
// ***   4. if arg1 and arg2: 
// ***       4.1 arg1 is a string
// ***       4.2 arg2 represents a boolean condition 
// ***           4.2.1 if [true|True|TRUE] the printed articles contains value of arg1 in its title
// ***           4.2.2 otherwise the printed article does not contain value of arg1 in its title
// ***   5. with no arguments: print latest 5 articles
// ***   6. There appears to an intention to deal with a concurrency situation which is not exactly clear
// ***      6.1 Perhaps a story may get added while the app is computing the articles to be printed???
// ***          This may be valid if the stories were getting loaded asynchronously, however it seems
// ***          they get loaded at the beginning.

// *** General comments on implemenation:
// *** 1. Use existing library classes available in .net to read RSS feed
// *** 2. Use existing library classes available in .net to filter RSS feed
// *** 3. It seems app is dealing with concurrency when it seems unnecessary
// *** 4. There is a roundabout way of printing the desired output - 
// ***      - first a large number of stories are loaded
// ***      - each story is assigned a random index as a key into a hash table
// ***      - the desired stories are retrieved from the list using some filter, 
// ***        then their random indices added to a queue, then retrieved using these
// ***        indices from the hash table. Why not just use existing XPath class to
// ***        filter the feed to get desired output
// *** 5. There is no clear boundaries between data, output and business logic (MVC). 
// ***    Although functionality is simple, it would still be better to follow an MVC pattern here
namespace RssReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // *** Argument passed to new RssReader is hard coded, perhaps default value should be defined in RssReader itself?
            // *** Besides, this argument is useless as the constructor uses another feed which is also hard coded.
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

            // *** No indication to the user what to do .. user will see some results and then just stare
            // *** Then they will type some key and program will exit, which is pretty confusing.
            // *** At least say 'Press any key to exit'
            Console.ReadLine();
            string s = "hello";
            if (s.Contains(""))
            {
                Console.WriteLine("non-empty string contains empty string");
            }
            else
            {
                Console.WriteLine("non-empty string does not contain empty string");
            }
            Console.ReadLine();
        }
    }

    public class RssStory
    {
        // *** public property name starting with _ does not look right
        // *** follow a camel-case notation
        public int _index { get; set; }
        // *** no consistency of property definitions
        // *** it seems there is _title and Title, both public and one is a getter-setter
        // *** Have just one getter-setter for each property
        public string _title;
        public string Title { get; set; }

        private string _description;
        public string Description { get; set; }

        public string _link;
        public Uri Link { get; set; }

        public DateTime Published { get; set; }

        // *** Not used at all
        Regex regex = new Regex("(?<scheme>[a-z]{3,5})://(?<host>[a-z0-9_-]+(.[a-z0-9_-]+)*)/(?<path>.*)");

        public RssStory(string title, string description, string link)
        {
            Title = title;
            Description = description;
            _link = link;

            // *** Delete experimental code, add when actually implementing.
            //Match match = regex.Match(link);
            //Link = new UriBuilder(match.Groups["scheme"].Value, match.Groups["host"].Value, -1, match.Groups["path"].Value).Uri;

            // 10,000 should be sufficient to avoid collisions.
            // *** This does not make sense, why is an index a random number?
            _index = new Random().Next(10000);
        }

        // *** Better design would be have an interface with a method like print()
        // *** and have this class implement that interface.
        public override string ToString()
        {
            Console.WriteLine("Title: " + Title);
            Console.WriteLine("Description: " + Description);
            Console.WriteLine("Published On: " + Published);
            Console.WriteLine("Link: " + _link);
            Console.WriteLine();

            // *** Why do we need a return value? It does not seem to be used by caller.
            return string.Empty;
        }
    }
}
