using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;

namespace NewsStories
{
    namespace View
    {
        public class UserInput
        {
            public NameValueCollection Options { get; private set; }
            private string usage;
            private string help;
            public UserInput(string[] args)
            {
                usage = "NewsStories [/with[out] \"string\"] [/latest count]";
                help =
@"
  /with     ""string""  display news articles containing ""string""
  /without  ""string""  display news articles not containing ""string""
  /latest   N           display latest N articles if no search criteria specified
                        display at most N latest articles satisfying search criteria
";
                if (!argsToOptions(args))
                {
                    throw new Exception(usage + help);
                }
            }
            private bool argsToOptions(string[] args) 
            {
                Options = new NameValueCollection();
                bool goodInput = true;
                if (args.Length > 0)
                {
                    string[] validOptions = { "/with", "/without", "/latest" };
                    if (args.Contains("/with") && args.Contains("/without"))
                    {
                        goodInput = false;
                    }
                    else 
                    {
                        if (args.Length == 2 || args.Length == 4)
                        {
                            for (int i = 0; i < args.Length / 2; i++)
                            {
                                int optionIndex = i * 2;
                                if (!validOptions.Contains(args[optionIndex]))
                                {
                                    goodInput = false;
                                    break;
                                }
                                if (Options[args[i * 2]] == null)
                                {
                                    Options.Add(args[i * 2], args[i * 2 + 1]);
                                }
                                else
                                {
                                    goodInput = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            goodInput = false;
                        }
                    }
                }
                return goodInput;
            }
        }
    }
}
