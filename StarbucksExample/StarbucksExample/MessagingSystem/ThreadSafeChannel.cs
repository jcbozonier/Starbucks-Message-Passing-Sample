using System;
using System.Collections;
using System.Threading;

namespace StarbucksExample.MessagingSystem
{
    ///
    /// Summary description for ThreadSafeQueue.
    ///
    public class ThreadSafeChannel : IChannel
    {
        private Queue q = Queue.Synchronized(new Queue());
        private readonly ManualResetEvent newItemEntered = new ManualResetEvent(false);

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
                var result = q.Dequeue();
                if (q.Count == 0)
                    newItemEntered.Reset();

                return result;
            }

        }
    }
}