using System;
using TwitterLib;
using System.Security;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
namespace com.intridea.presently
{
    public class TwitterService
    {
        public TwitterService(PresentlyWebPart webpart)
        {
            _webpart = webpart;
            updateLogins(webpart.Username, webpart.Password, webpart.Url);
        }

        String _lastId;
        PresentlyWebPart _webpart;
        TwitterNet twitter;
        public String LastId
        {
            get { return _lastId; }
            set { _lastId = value; }
        }
        public bool isConfigured()
        {
            return twitter != null;
        }
        public void updateLogins(String user, String password, String domain)
        {
            if (domain != null && user != null && password != null)
            {
                SecureString spassword = getPasswordAsSecureString(password);
                twitter = new TwitterNet(user, spassword);
                twitter.TwitterServerUrl = "https://" + domain + "/api/twitter/";
                twitter.ClientName = "SharePoint";
            }
            else
            {
                twitter = null;
            }
        }
        public void SendTweet(String tweet)
        {
            if (twitter == null)
                return;
            try
            {
                twitter.AddTweet(tweet);
            }
            catch (System.Net.WebException we)
            {
                twitter.TwitterServerUrl = twitter.TwitterServerUrl.Replace("https://", "http://");
                try
                {
                    twitter.AddTweet(tweet);
                }
                catch (Exception err)
                {
                }

            }
            catch (Exception err)
            {
            }
        }
        public TweetCollection GetTweets()
        {
            TweetCollection tweets = new TweetCollection();
            Tweet tweet = new Tweet();
            if (twitter == null)
            {
                tweet.Text = "Please setup your user/password and url in the connection settings.";
                tweets.Add(tweet);
                return tweets;
            }
            try
            {
                //if (_lastId == null)
                tweets = twitter.GetFriendsTimeline();
                /*else
                    tweets = twitter.GetFriendsTimeline(_lastId.ToString());
                if (tweets != null && tweets.Count > 0)
                {
                    _lastId = String.Format("{0:0}", tweets[0].Id);
                }
                else
                {
                    tweet.Text = "sent " + _lastId;
                    tweets.Add(tweet);
                }*/
                return tweets;
            }
            catch (System.Net.WebException we)
            {
                twitter.TwitterServerUrl = twitter.TwitterServerUrl.Replace("https://","http://");
                try
                {
                    return twitter.GetFriendsTimeline();
                }
                catch (Exception err)
                {
                    tweet.Text = err.Message;
                    tweets.Add(tweet);
                    return tweets;
                }
 
            }
            catch (Exception err)
            {
                tweet.Text = err.Message;
                tweets.Add(tweet);
                return tweets;
            }
        }

        private static SecureString getPasswordAsSecureString(string password)
        {
            SecureString securePassword = new SecureString();

            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }
            return securePassword;
        }
    }
}
