using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FactoryTest
{
    [TestClass]
    public class UnitTestCreateStories
    {
        [TestMethod]
        public void TestCreateStories()
        {
            int nStories = 5;
            string searchTerm = "hat";
            Dictionary<string, object> conditions = new Dictionary<string, object>();
            conditions.Add("searchTerm", searchTerm);
            conditions.Add("tailSize", nStories);
            NewsArticles.Model.IStories rssStories = NewsArticles.Controller.StoriesFactory.produce("RSS", "ABC-MOST-READ", conditions);
            Assert.IsNotNull(rssStories);


            nStories = 5;
            searchTerm = "hat";
            conditions = new Dictionary<string, object>();
            conditions.Add("searchTerm", searchTerm);
            conditions.Add("tailSize", nStories);
            NewsArticles.Model.IStories atomStories = NewsArticles.Controller.StoriesFactory.produce("ATOM", "USGS-ALL-QUAKES", conditions);
            Assert.IsNotNull(atomStories);
        }
    }
}
