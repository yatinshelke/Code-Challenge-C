using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader
{
    // Ready for DI.
    // *** Either this interface is not needed or RssReader should implement this interface
    // *** An interface makes sense if there will be multiple implementations of the interface
    // *** or if there is intention of additional implementations
    // *** E.g. one implementation uses algorithm X to get top stories and another uses algorithm Y
    public interface IRssReader
    {
        void LoadStories();
        IEnumerable<RssStory> GetTopStories();
    }
}
