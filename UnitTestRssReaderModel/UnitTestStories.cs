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
            Stories storiesBBC = new Stories("BBC");
            Assert.IsNotNull(storiesBBC);
            Stories storiesCNN = new Stories("CNN");
            Assert.IsNotNull(storiesCNN);
        }
    }
}
