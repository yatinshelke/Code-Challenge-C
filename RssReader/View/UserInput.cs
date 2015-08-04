using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;


namespace NewsArticles.View
{
    public class UserInput
    {
        public NameValueCollection Options { get; private set; }
        private string usage;
        private string help;
        private string[] feedSections = { "feeds/rss", "feeds/atom" };
        public UserInput(string[] args)
        {
            usage =
@"NewsArticles [/with[out] searchTerm] [/latest count] [feedName]
NewsArticles [/with[out] searchTerm] [/latest count] [/rss|/atom feedUri]";
            help =
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
            if (!argsToOptions(args))
            {
                throw new Exception(usage + help);
            }
        }
        private bool isKnownFeed(string feedName, out string feedType)
        {
            bool isValid = false;
            feedType = "";
            foreach (string section in feedSections)
            {
                var knownFeeds = ConfigurationManager.GetSection(section) as NameValueCollection;
                if (knownFeeds != null && knownFeeds[feedName] != null)
                {
                    isValid = true;
                    if (section.Equals("feeds/rss"))
                    {
                        feedType = "RSS";
                    }
                    else if (section.Equals("feeds/atom"))
                    {
                        feedType = "ATOM";
                    }
                    else
                    {
                        throw new Exception("App Configuration error");
                    }
                    break;
                }
            }
            return isValid;
        }
        private bool isFeedValidUri(string uri)
        {
            bool isValid = false;
            Uri uriResult;
            if (Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
            {
                if (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        private bool isFeedIdentifierValid(string feedId)
        {
            string feedType;
            return isKnownFeed(feedId, out feedType) || isFeedValidUri(feedId) || false;            
        }
        private bool argsToOptions(string[] args)
        {
            // TODO: Need a good command line options processor so that we can avoid
            // complicated processing here!
            Options = new NameValueCollection();
            if (args.Length > 6)
            {
                return false;
            }
            if (args.Length == 0)
            {
                string defaultFeedName = ConfigurationManager.AppSettings.Get("defaultFeedName");
                if (defaultFeedName == null)
                {
                    defaultFeedName = "BBC";
                }
                if (defaultFeedName != null) {
                    Options.Add("feedIdentifier", defaultFeedName);
                    Options.Add("feedType", "RSS");
                }
            }
            if (args.Length > 0)
            {
                string[] optionArgs;
                if (args.Length % 2 != 0)
                {
                    string feedIdentifier = args[args.Length - 1];
                    string feedType;
                    if (isKnownFeed(feedIdentifier, out feedType))
                    {
                        optionArgs = args.Take(args.Length - 1).ToArray<string>();
                        Options.Add("feedIdentifier", feedIdentifier);
                        Options.Add("feedType", feedType);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    optionArgs = args;
                }
                string[] validOptions = { "/with", "/without", "/latest", "/rss", "/atom" };
                if (optionArgs.Contains("/with") && optionArgs.Contains("/without"))
                {
                    return false;
                }
                if (optionArgs.Contains("/rss") && optionArgs.Contains("/atom"))
                {
                    return false;
                }
                string[] optionNames = optionArgs.Where((arg, index) => index % 2 == 0).ToArray();
                if (optionNames.Distinct().Count() != optionNames.Length)
                {
                    return false;
                }

                if (optionArgs.Length % 2 == 0)
                {
                    for (int i = 0; i < optionArgs.Length / 2; i++)
                    {
                        int optionIndex = i * 2;
                        if (!validOptions.Contains(optionArgs[optionIndex]))
                        {
                            return false;
                        }
                        if (Options[optionArgs[i * 2]] == null)
                        {
                            Options.Add(optionArgs[i * 2], args[i * 2 + 1]);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (Options["/latest"] != null)
                    {
                        int latestValue;
                        if (!Int32.TryParse(Options["/latest"], out latestValue))
                        {
                            return false;
                        }
                    }
                    if (Options["/rss"] != null)
                    {
                        if (!isFeedValidUri(Options["/rss"]))
                        {
                            return false;
                        }
                        Options["feedIdentifier"] = Options["/rss"];
                        Options["feedType"] = "RSS";
                    }  
                    else if (Options["/atom"] != null) 
                    {
                        if (!isFeedValidUri(Options["/atom"]))
                        {
                            return false;
                        }
                        Options["feedIdentifier"] = Options["/atom"];
                        Options["feedType"] = "ATOM";
                    }
                }
                else 
                {
                    return false;
                }
            }
            return true;
        }
    }
}
