using System;
using System.Collections.Generic;
using System.Text;
using TwitterLib;
namespace com.intridea.presently
{
    class TweetBuilder
    {
    public static String buildTweets(TweetCollection tc) {
        try {
            if (tc.Count == 0) return "no tweets";
            StringBuilder allSb = new StringBuilder();
            int i = 0;
            foreach (Tweet tweet in tc)
            {
                StringBuilder sb = new StringBuilder();
                int insertion_index = 0;
                String style = (i % 2 == 0) ? "even_color" : "odd_color";
                if (tweet.User != null)
                {
                    sb.Append("<div class='tweet " + style + "'>");
                    insertion_index = sb.Length;
                    sb.Append("<div class='tweetUserName'><a class='user_link' rel='@" + tweet.User.ScreenName + "'>" + "<img src='" + tweet.User.ImageUrl + "'/>" + "</a></div>");
                }
                String replyTo = "";
                string strTweet = parseTweet(tweet.Text, out replyTo);
                sb.Append("<div id='"+tweet.Id+"' class='tweetText'>" + strTweet);
                if (tweet.Paste != null )
                {
                    sb.Append("<a class='more_text_link'>  More..</a>");
                    sb.Append("<div class='more_text'>");
                    sb.Append(parseTweet(tweet.Paste.Text, out replyTo));
                    sb.Append("</div>");
                }
                if (tweet.Assets != null && tweet.Assets.Count > 0)
                {
                    sb.Append("<div class='tweetAttachment'><span>Attachments:</span>");
                    foreach (Attachment attachment in tweet.Assets)
                    {
                        String url = attachment.Url.Replace("/original/", "/stream_multi_thumb/");
                        if (attachment.ContentType.StartsWith("image"))
                            sb.Append("<a rel='lightbox' href='" + attachment.Url + "'><img src='" + url + "'/></a>");
                        else
                            sb.Append("<a href='" + attachment.Url + "'>" + attachment.FileName + "</a>");
                    }
                    sb.Append("</div>");
                }
                sb.Append("</div>");

                if (tweet.User != null)
                {
                    if (replyTo != null)
                        sb.Insert(insertion_index, "<div class=\"tweetNote\">" + tweet.User.FullName + " To " + replyTo + "</div>");
                    else if (tweet.User != null)
                        sb.Insert(insertion_index, "<div class=\"tweetNote\">" + tweet.User.FullName + "</div>");
                }
                if (tweet.RelativeTime != null)
                    sb.Append("<div class=\"tweetRelativeTime\">" + tweet.RelativeTime + " " + tweet.Source + "</div>");
                
                i++;
                sb.Append("<div style='clear:both'></div></div>");
                allSb.Append(sb.ToString());
            }
            return allSb.ToString();
        }
        catch (Exception err)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>" + err.Message + "</div>" + "<div>" + err.StackTrace + "</div>");
            return sb.ToString();
        }        
    }
    private static string parseTweet(string tweet, out String replyTo)
    {
        string strParsedTweets = string.Empty;
        string[] arrTweet = tweet.Split(' ');
        replyTo = null;
        foreach (string strTweet in arrTweet)
        {
            string strParsedTweet;

            if (strTweet.StartsWith("http://") || strTweet.StartsWith("https://"))
            {
                strParsedTweet = "<a href=\"" + strTweet + "\">" + strTweet + "</a>";
            }
            else if (strTweet.StartsWith("@"))
            {
                strParsedTweet = "<a class='user_link'  rel='" + strTweet + "'>" + strTweet + "</a>";
                replyTo = strTweet.Substring(1);
            }
            else
            {
                strParsedTweet = strTweet;
            }

            strParsedTweets += " " + strParsedTweet.Replace("\n", "<br/>");
        }
        return strParsedTweets;

    }
  }

}
