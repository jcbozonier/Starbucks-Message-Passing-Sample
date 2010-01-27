using System;
using System.Threading;

namespace StarbucksExample.MessagingSystem
{
    ///
    /// Summary description for ThreadSafeQueue.
    ///
    public class ThreadSafeChannel : IChannel
    {
        private System.Collections.Queue q = new System.Collections.Queue();
        private readonly ManualResetEvent newItemEntered = new ManualResetEvent(false);

        public ThreadSafeChannel()
        {
        }

        public void Enqueue(object o)
        {
            lock (this)
            {
                q.Enqueue(o);
                newItemEntered.Set();
            }
        }

        public object Dequeue()
        {
            newItemEntered.WaitOne();

            lock (this)
            {
                object result = q.Dequeue();
                if (q.Count == 0)
                    newItemEntered.Reset();

                return result;
            }

        }
    }
}