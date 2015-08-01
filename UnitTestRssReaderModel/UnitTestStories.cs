using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RssReader.Model;

namespace UnitTestRssReaderModel
{
    [TestClass]
    public class UnitTestStories
    {
        [TestMethod]
        public void TestStoriesConstructor()
        {
            string[] validTestFeedNames = new string[] {"BBC", "CNN"};

            for (int i = 0; i < validTestFeedNames.Length; i++)
            {
                Stories stories = new Stories(validTestFeedNames[i], "");
                Assert.IsNotNull(stories);
            }

            string[] invalidTestFeedNames = new string[] { "XYZ", "FOO", "BAR" };

            for (int i = 0; i < invalidTestFeedNames.Length; i++)
            {
                bool failedOnInvalidRSSIdentifier = false;
                try
                {
                    Stories stories = new Stories(invalidTestFeedNames[i], "");
                }
                catch (Exception e)
                {
                    failedOnInvalidRSSIdentifier = true;
                }
                Assert.IsTrue(failedOnInvalidRSSIdentifier);
            }

            // these just have to be valid http or https URI's, not necessarily RSS feeds
            string[] validUris = new string[] {
                "http://www.cnn.com",
                "https://gmail.com",
                "http://feeds.abcnews.com/abcnews/internationalheadlines",
                "http://feeds.bbci.co.uk/news/rss.xml",
            };
            for (int i = 0; i < validUris.Length; i++)
            {
                Stories stories = new Stories(validUris[i], "");
                Assert.IsNotNull(stories);
            }
            string[] invalidUris = new string[] {
                "http:///www.cnn.com",
                "https:/gmail.com",
                "ftp://feeds.abcnews.com/abcnews/internationalheadlines",
                "file://feeds.bbci.co.uk/news/rss.xml",
            };
            for (int i = 0; i < invalidUris.Length; i++)
            {
                bool failedOnInvalidRSSIdentifier = false;
                try
                {
                    Stories stories = new Stories(invalidUris[i], "");
                }
                catch (Exception e)
                {
                    failedOnInvalidRSSIdentifier = true;
                }
                Assert.IsTrue(failedOnInvalidRSSIdentifier);
            }
        }
        [TestMethod]
        public void TestStoriesRead()
        {
            Stories stories = new Stories("ABC-MOST-READ", "//channel/item[contains(title, 'hat')][position()>last()-2]");
            string text = stories.read();
            Assert.IsNull(text);
        }
    }
}
