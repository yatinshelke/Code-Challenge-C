using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace RssReader
{
    public class RssReader
    {
        // *** Do not hard code this, take it from config file if intention is to be a constant
        // *** or take it from arguments, either as URI or a string that maps to a URI
        // *** e.g. run app like this: RssReader http://feeds.bbci.co.uk/news/rss.xml --contains "London"
        // ***      or RssReader BBC --contains "London"
        // *** App can contain a mapping of popular news feeds so app user can type a friendly name for the feed
        private const string FEED_ADDRESSS = "http://feeds.bbci.co.uk/news/rss.xml";

        // *** Perhaps we do not need 3 data structures which contain common data
        // *** TODO: simplify algorithm to get latest stories
        private Dictionary<int, RssStory> _storyLookup;
        private ThreadSafeQueue _storyIndex;
        private List<RssStory> _stories;
        private XElement _feed;

        // Constructors
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="feedAddress"></param>
        public RssReader(string feedAddress)
        {
            reader = XmlReader.Create(feedAddress);
            feedDocument = new XPathDocument(XmlReader.Create(feedAddress));
            _feed = XElement.Load(FEED_ADDRESSS).Element("channel");
            _storyLookup = new Dictionary<int, RssStory>();
            _storyIndex = new ThreadSafeQueue();
            _stories = new List<RssStory>();
        }

        #endregion

        /**********************
        *  ALL STORIES MUST BE
        *  LOADED PRIOR TO USE.
        ***********************/
        // *** If all stories must be loaded prior to use, why is there a need for a thread-safe queue to store the latest stories?
        // *** Also, why load all the stories and then figure out the latest stories? Why not use XPath to get the latest
        // *** stories from the feed based on search criteria?
        public void LoadStories()
        {
            foreach (var item in _feed.Elements("item"))
            {
                _stories.Add(new RssStory(item.Element("title").Value, item.Element("description").Value, item.Element("link").Value));
                _stories.Last().Published = DateTime.Parse(item.Element("pubDate").Value);

                if (_storyLookup.ContainsKey(_stories.Last()._index))
                {
                    // Story is already in the list.
                    // Not sure why this happens, but we should remove it.
                    // *** There are duplicates of _index because when create the RssStory, it internally assigns a random number
                    // *** to _index, so two stories may end up with the same value of _index
                    // *** It is not clear what the purpose of _index is. The fact that it is random goes against the idea of
                    // *** naming it "_index"
                    _stories.Remove(_stories.Last());
                }
                else
                {
                    _storyLookup.Add(_stories.Last()._index, _stories.Last());
                }
            }
        }

        public IEnumerable<RssStory> GetTopStories()
        {
            var latestStories = _stories.OrderBy(s => s.Published).Reverse();
            var recentStories = new List<RssStory>();

            // Get the five most recent stories from the feed.
            // *** How is this getting the 5 latest stories??
            // *** It seems to be getting stories that are published before 30 minutes from now and returning a list of their _index
            // *** which happens to be a random number.. so the whole app needs to be rewritten
            // *** Suggest using XPath to get the latest N stories satisfying criteria
            var fiveLatestStoryIndices = latestStories.TakeWhile(s => s.Published < DateTime.Now.AddMinutes(30)).Select(s => s._index);

            foreach (int index in fiveLatestStoryIndices)
            {
                // *** which other thread is doing this to cause a concurrency conflict???
                // *** Besides, there are classes like ConcurrentQueue<T> which can be used instead of implementing
                // *** our own thread safe queue
                _storyIndex.Enqueue(index);
            }

            while (true)
            {
                try
                {
                    // *** Why the roundabout way of initially creating a list of items, then making a queue of their indices,
                    // *** then retrieving the same item from a hash table with the index as the key???
                    // *** The app can be simplified using XPath
                    recentStories.Add(_storyLookup[_storyIndex.Pop()]);
                }
                catch (Exception)
                {
                    // Queue is empty.
                    break;
                }
            }

            return recentStories;
        }
    }
}
