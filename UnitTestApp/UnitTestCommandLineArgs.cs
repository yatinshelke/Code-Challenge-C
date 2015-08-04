using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsArticles;

namespace UnitTestApp
{
    [TestClass]
    public class UnitTestCommandLineArgs
    {
        private bool run(string[] args)
        {
            NewsArticles.View.UserInput input = null;
            try
            {
                input = new NewsArticles.View.UserInput(args);
            }
            catch (Exception e)
            {
                return false;
            }
            try
            {
                NewsArticles.Controller.Printer printer = new NewsArticles.Controller.Printer(input);
                printer.print();
            }
            catch (Exception e)
            {
                int i;
                return false;
            }
            return true;
        }
        [TestMethod]
        public void TestRun()
        {
            Assert.IsTrue(run(new string[]{"BBC"}));
            Assert.IsTrue(run(new string[] { "/latest", "1", "BBC" }));
            Assert.IsTrue(run(new string[] { "USGS-ALL-QUAKES" }));
            Assert.IsTrue(run(new string[] { "/with", "California", "USGS-ALL-QUAKES" }));
        }
    }
}
