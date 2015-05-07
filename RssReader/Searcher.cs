using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader
{
    /// <summary>
    /// Ensures that only stuff being searched for is returned.
    /// </summary>
    public class Searcher
    {
        public bool not = false;
        public String s = "";

        public Searcher(string[] args)
        {
            try
            {
                s = args[1] ?? " ";
                not = args[2] == "true" ? true : args[2] == "True" ? true : args[2] == "TRUE" ? true : false;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                s = " ";
                not = false;
            }
            catch (Exception exception)
            {
                // more detailed message when something goes wrong in debug mode
#if DEBUG
                Console.WriteLine("Could not parse search parameters. There are " + args.Length + " parameters");
#else
                Console.WriteLine(args[0]);
#endif
                s = " ";
                not = false;
            }
            finally
            {
                s = s != " " ? s : "";
            }
        }

        /// <summary>
        /// Check and see if input matches conditions.
        /// </summary>
        /// <param name="toSearch"></param>
        /// <returns>Returns true or throws an exception.</returns>
        public bool SearchItHard(string toSearch)
        {
            bool matches = (toSearch.Contains(s) && !not) || (!toSearch.Contains(s) && not);

            if(matches == false)
            {
                throw new Exception("Doesn't match!");
            }

            return true;
        }
    }
}
