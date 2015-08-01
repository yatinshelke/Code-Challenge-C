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
        public UserInput(string[] args)
        {
            usage = "NewsArticles [/with[out] \"string\"] [/latest count] [feedName|feedUri]";
            help =
@"
  /with     ""string""  display news articles containing ""string""
  /without  ""string""  display news articles not containing ""string""
  /latest   N           display latest N articles if no search criteria specified
                        display at most N latest articles satisfying search criteria
  feedName              Recognized Feed names (Defaults to BBC):
                            BBC                British Broadcasting Network
                            CNN                Cable News Network
                            ABC-INT            ABC International
                            ABC-MOST-READ      ABC Most read stories
  feedUri               The URI of the news feed
";
            if (!argsToOptions(args))
            {
                throw new Exception(usage + help);
            }
        }
        private bool isFeedIdentifierValid(string uri)
        {
            bool isValid = false;
            var knownFeeds = ConfigurationManager.GetSection("feeds/rss") as NameValueCollection;
            if (knownFeeds != null && knownFeeds[uri] != null)
            {
                isValid = true;
            }
            else
            {
                Uri uriResult;
                if (Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                {
                    if (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
        private bool argsToOptions(string[] args)
        {
            // TODO: Need a good command line options processor so that we can avoid
            // complicated processing here!
            Options = new NameValueCollection();
            if (args.Length > 5)
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
                }
            }
            if (args.Length > 0)
            {
                string[] optionArgs;
                if (args.Length == 1 || args.Length == 3 || args.Length == 5)
                {
                    string feedIdentifier = args[args.Length - 1];
                    if (isFeedIdentifierValid(feedIdentifier))
                    {
                        optionArgs = args.Take(args.Length - 1).ToArray<string>();
                        Options.Add("feedIdentifier", feedIdentifier);
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
                string[] validOptions = { "/with", "/without", "/latest" };
                if (optionArgs.Contains("/with") && optionArgs.Contains("/without"))
                {
                    return false;
                }
                else
                {

                    if (optionArgs.Length == 2 || optionArgs.Length == 4)
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
                    }
                }
            }
            return true;
        }
    }
}
