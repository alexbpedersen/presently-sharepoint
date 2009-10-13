using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.intridea.presently;

namespace TwitterTest
{
    /// <summary>
    /// Summary description for TwitterServiceTest
    /// </summary>
    [TestClass]
    public class TwitterServiceTest
    {
        public TwitterServiceTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestGetTweets()
        {
            com.intridea.presently.PresentlyWebPart part = new com.intridea.presently.PresentlyWebPart("ping","test123", "intridea.presentlyapp.com");
            String tweets = part.GetTweets();
            Assert.IsTrue(tweets.Length > 0);
        }
        [TestMethod]
        public void TestGetTweetsWithoutSettings()
        {
            com.intridea.presently.PresentlyWebPart part = new com.intridea.presently.PresentlyWebPart();
            String tweets = part.GetTweets();
            Assert.IsTrue(tweets.Length > 0);
        }
        [TestMethod]
        public void TestSendTweets()
        {
            com.intridea.presently.PresentlyWebPart part = new com.intridea.presently.PresentlyWebPart("ping", "test123", "intridea.presentlyapp.com");
            TwitterService ts = new TwitterService(part);
            ts.LastId = "935793";
            TwitterLib.TweetCollection tweets = ts.GetTweets();
            //ts.SendTweet("d @ping test");
            TwitterLib.TweetCollection tweets2 = ts.GetTweets();
            Assert.IsTrue(tweets2.Count > 0);
        }
    }
}
