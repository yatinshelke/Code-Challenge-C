using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsArticles.View;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace UnitTestUserInput
{
    [TestClass]
    public class UnitTestArgsProcessing
    {
            private string expectedUsage = 
@"NewsArticles [/with[out] searchTerm] [/latest count] [feedName]
NewsArticles [/with[out] searchTerm] [/latest count] [/rss|/atom feedUri]";

            private string expectedHelp =
@"
  /with     ""string""  display news articles containing ""string""
  /without  ""string""  display news articles not containing ""string""
  /latest   N           display latest N articles if no search criteria specified
                        display at most N latest articles satisfying search criteria
  /rss      ""string""  The URI of the RSS news feed (mutually exclusive with /atom)
  /atom     ""string""  The URI of the ATOM news feed (mutually exclusive with /rss)
  feedName              Recognized Feed names (Defaults to BBC RSS feed):
                            BBC                (RSS)   British Broadcasting Network
                            CNN                (RSS)   Cable News Network
                            ABC-INT            (RSS)   ABC International
                            ABC-MOST-READ      (RSS)   ABC Most read stories
                            USGS-ALL-QUAKES    (ATOM)  USGS All earthquakes in past hour
";
        private void checkValidArgs(string[] args)
        {
            bool caughtError = false;
            UserInput input = null;
            try
            {
                input = new UserInput(args);
            }
            catch (Exception e)
            {
                caughtError = true;
            }
            Assert.IsFalse(caughtError);
            Assert.IsTrue(input.Options["feedIdentifier"] != null);
        }       
        private void checkValidOptions(NameValueCollection options)
        {
            List<string> argsList = new List<string>();
            foreach (string key in options.AllKeys)
            {
                argsList.Add(key);
                argsList.Add(options.Get(key));
            }
            string[] args = argsList.ToArray();
            UserInput input = new UserInput(args);
            Assert.IsTrue(input.Options.Count == options.Count);
            foreach (string key in options.AllKeys)
            {
                Assert.IsNotNull(input.Options[key]);
                Assert.IsTrue(input.Options[key].Equals(options.Get(key)));
            }
        }
        [TestMethod]
        public void TestOptionFeedName()
        {
            List<string[]> argsSet = new List<string[]>();
            string[] goodArgs1 = { "BBC" };
            argsSet.Add(goodArgs1);
            string[] goodArgs2 = { "/rss", "http://feeds.abcnews.com/abcnews/internationalheadlines" };
            argsSet.Add(goodArgs2);
            string[] goodArgs3 = { "/latest", "5", "CNN"};
            argsSet.Add(goodArgs3);
            string[] goodArgs4 = { "/with", "hat", "/rss", "http://feeds.abcnews.com/abcnews/mostreadstories" };
            argsSet.Add(goodArgs4);
            string[] goodArgs5 = { "/with", "hat", "/latest", "5", "ABC-MOST-READ" };
            argsSet.Add(goodArgs5);
            string[] goodArgs6 = { "/with", "hat", "/latest", "5", "/rss", "http://feeds.bbci.co.uk/news/rss.xml" };
            argsSet.Add(goodArgs6);
            string[] goodArgs7 = { "USGS-ALL-QUAKES" };
            argsSet.Add(goodArgs7);

            foreach (string[] args in argsSet)
            {
                checkValidArgs(args);
            }
        }
        [TestMethod]
        public void TestOptionWith()
        {
            NameValueCollection options = new NameValueCollection();
            options.Add("/with", "hat");
            checkValidOptions(options);
        }
        [TestMethod]
        public void TestOptionLatest()
        {
            NameValueCollection options = new NameValueCollection();
            options.Add("/latest", "5");
            checkValidOptions(options);
        }
        [TestMethod]
        public void TestOptionsWithLatest()
        {
            NameValueCollection options = new NameValueCollection();
            options.Add("/with", "hat");
            options.Add("/latest", "5");
            checkValidOptions(options);
        }

        private void checkInvalidArgs(string[] args)
        {
            // This method will prevent developer from casually changing usage
            // Usage is very user visible and must be constructed carefully and
            // not changed without reason. If usage is changed, this unit test 
            // must be modified to check new usage.
            bool caughtError = false;
            try
            {
                UserInput input = new UserInput(args);
            }
            catch (Exception e)
            {
                string message = e.Message;
                string expectedMessage = expectedUsage + expectedHelp;
                Assert.IsTrue(e.Message.Equals(expectedUsage + expectedHelp));
                caughtError = true;
            }
            Assert.IsTrue(caughtError);
        }
        private void checkInvalidOptions(NameValueCollection options)
        {
            List<string> argsList = new List<string>();
            foreach (string key in options.AllKeys)
            {
                argsList.Add(key);
                argsList.Add(options.Get(key));
            }
            string[] args = argsList.ToArray();
            checkInvalidArgs(args);
        }
        [TestMethod]
        public void TestOptionsExclusive()
        {
            NameValueCollection options = new NameValueCollection();
            options.Add("/with", "hat");
            options.Add("/without", "bat");
            checkInvalidOptions(options);
        }
        [TestMethod]
        public void TestInvalidOptions()
        {

            NameValueCollection[] optionsSet = new NameValueCollection[2];

            optionsSet[0] = new NameValueCollection();
            optionsSet[0].Add("/foo", "hat");
            optionsSet[0].Add("/bar", "bat");

            optionsSet[1] = new NameValueCollection();
            optionsSet[1].Add("/ with", "hat");
            optionsSet[1].Add("/ without", "bat");

            for (int i = 0; i < optionsSet.Length; i++)
            {
                checkInvalidOptions(optionsSet[i]);
            }
        }
        [TestMethod]
        public void TestInvalidArgs()
        {
            List<string[]> argsSet = new List<string[]>();
            string[] badArgs1 = {"/with"};
            argsSet.Add(badArgs1);
            string[] badArgs2 = { "/without" };
            argsSet.Add(badArgs2);
            string[] badArgs3 = { "/latest" };
            argsSet.Add(badArgs3);
            string[] badArgs4 = { "/with", "hat", "/latest" };
            argsSet.Add(badArgs4);
            string[] badArgs5 = { "/with", "hat", "/latest", "5", "/with", "foo"};
            argsSet.Add(badArgs5);
            string[] badArgs6 = { "hello", "world"};
            argsSet.Add(badArgs6);
            string[] badArgs7 = { "http://feeds.abcnews.com/abcnews/internationalheadlines" };
            argsSet.Add(badArgs7);
            string[] badArgs8 = { "/with", "hat", "http://feeds.abcnews.com/abcnews/mostreadstories" };
            argsSet.Add(badArgs8);
            string[] badArgs9 = { "/with", "hat", "/latest", "5", "http://feeds.bbci.co.uk/news/rss.xml" };
            argsSet.Add(badArgs9);

            foreach (string[] args in argsSet)
            {
                checkInvalidArgs(args);
            }
        }
    }
}
