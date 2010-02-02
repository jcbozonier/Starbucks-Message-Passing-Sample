using System;
using System.Collections;
using System.Threading;
using StarbucksExample.Messages;

namespace StarbucksExample.MessagingSystem
{
    ///
    /// Summary description for ThreadSafeQueue.
    ///
    public class ThreadBlockingChannel : IEnqueue, IDequeue
    {
        private Queue q = Queue.Synchronized(new Queue());
        private readonly ManualResetEvent newItemEntered = new ManualResetEvent(false);

        public void Enqueue(IMessage o)
        {
            lock (this)
            {
                q.Enqueue(o);
                newItemEntered.Set();
            }
        }

        public IMessage Dequeue()
        {
            newItemEntered.WaitOne();

            lock (this)
            {
                var result = q.Dequeue();
                if (q.Count == 0)
                    newItemEntered.Reset();

                return (IMessage)result;
            }

        }
    }
}