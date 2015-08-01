using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewsReader.Model;

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
namespace NewsReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // TODO: Implement Controller and View code
            // Model already provides a list of stories as List<StoryContent>
            // This data needs to be converted by the Controller code so
            // that it can be displayed in the view (which is the console)
            Stories feedStories = new Stories("BBC", "");
        }
    }

}
