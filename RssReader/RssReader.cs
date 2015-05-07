using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RssReader
{
    public class RssReader
    {
        private const string FEED_ADDRESSS = "http://feeds.bbci.co.uk/news/rss.xml";

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
            var fiveLatestStoryIndices = latestStories.TakeWhile(s => s.Published < DateTime.Now.AddMinutes(30)).Select(s => s._index);

            foreach (int index in fiveLatestStoryIndices)
            {
                _storyIndex.Enqueue(index);
            }

            while (true)
            {
                try
                {
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
