using System.Collections.Generic;
using System.Linq;

namespace RssReader
{
    /// <summary>
    /// Need a queue that is threadsafe. This should be that collection.
    /// </summary>
    public class ThreadSafeQueue
    {
        /*~ Mutex ~*/
        private object _lock;

        /// <summary>
        /// Backing data store is constant.
        /// </summary>
        private List<int> BACKING_LIST;

        public ThreadSafeQueue() {
            BACKING_LIST = new List<int>();
        }

        /// <summary>
        /// Add value to the queue.
        /// </summary>
        /// <param name="value">Float value to add</param>
        public void Enqueue(int value)
        {
            BACKING_LIST.Add(value);
            _lock = new object();
        }

        /// <summary>
        /// Return and remove a value from the queue.
        /// </summary>
        /// <param name="defaultValue">Default to return if queue is empty.</param>
        /// <returns>Number.</returns>
        public int Pop()
        {
            int value; // Store an int

            lock(_lock) { // Thread safety for the win
                value = BACKING_LIST.Take(BACKING_LIST.Count).Last(); // Take the leading value in the queue
            }

            BACKING_LIST.Remove(value); // Make sure to remove that value.
            return value; // Return the value
        }
    }
}
