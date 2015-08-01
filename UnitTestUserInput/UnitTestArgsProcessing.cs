using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsStories.View;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace UnitTestUserInput
{
    [TestClass]
    public class UnitTestArgsProcessing
    {
        private string usage = "NewsStories [/with[out] \"string\"] [/latest count]";
        private string help =
@"
  /with     ""string""  display news articles containing ""string""
  /without  ""string""  display news articles not containing ""string""
  /latest   N           display latest N articles if no search criteria specified
                        display at most N latest articles satisfying search criteria
";
        [TestMethod]
        public void TestOptionWith()
        {
            NameValueCollection options = new NameValueCollection();
            options.Add("/with", "hat");
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
        public void TestOptionLatest()
        {
            NameValueCollection options = new NameValueCollection();
            options.Add("/latest", "5");
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
        public void TestOptionsWithLatest()
        {
            NameValueCollection options = new NameValueCollection();
            options.Add("/with", "hat");
            options.Add("/latest", "5");
            List<string> argsList = new List<string>();
            foreach (string key in options.AllKeys)
            {
                argsList.Add(key);
                argsList.Add(options.Get(key));
            }
            string[] args = argsList.ToArray(); 
            UserInput input = new UserInput(args);
            Assert.IsTrue(input.Options.Count == 2);
            foreach (string key in options.AllKeys)
            {
                Assert.IsNotNull(input.Options[key]);
                Assert.IsTrue(input.Options[key].Equals(options.Get(key)));
            }
        }
        [TestMethod]
        public void TestOptionsExclusive()
        {
            // This test will prevent developer from casually changing usage
            // Usage is very user visible and must be constructed carefully and
            // not changed without reason. If usage is changed, this unit test 
            // must be modified to check new usage.

            NameValueCollection options = new NameValueCollection();
            options.Add("/with", "hat");
            options.Add("/without", "bat");
            List<string> argsList = new List<string>();
            foreach (string key in options.AllKeys)
            {
                argsList.Add(key);
                argsList.Add(options.Get(key));
            }
            string[] args = argsList.ToArray();
            bool caughtExclusiveOptions = false;
            try
            {
                UserInput input = new UserInput(args);
            }
            catch (Exception e)
            {
                string message = e.Message;
                string expectedMessage = usage + help;
                Assert.IsTrue(e.Message.Equals(usage + help));
                caughtExclusiveOptions = true;
            }
            Assert.IsTrue(caughtExclusiveOptions);
        }
        private void checkInvalidOptions(NameValueCollection options)
        {
            // This method will prevent developer from casually changing usage
            // Usage is very user visible and must be constructed carefully and
            // not changed without reason. If usage is changed, this unit test 
            // must be modified to check new usage.
            List<string> argsList = new List<string>();
            foreach (string key in options.AllKeys)
            {
                argsList.Add(key);
                argsList.Add(options.Get(key));
            }
            string[] args = argsList.ToArray();
            bool caughtError = false;
            try
            {
                UserInput input = new UserInput(args);
            }
            catch (Exception e)
            {
                string message = e.Message;
                string expectedMessage = usage + help;
                Assert.IsTrue(e.Message.Equals(usage + help));
                caughtError = true;
            }
            Assert.IsTrue(caughtError);
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
    }
}
