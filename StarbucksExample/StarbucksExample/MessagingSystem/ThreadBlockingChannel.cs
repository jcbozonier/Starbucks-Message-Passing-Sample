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

    public class EnumerableChannel : IEnqueue, IEnumerable, IEnumerator
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

        object _Dequeue()
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

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            Current = _Dequeue();

            return !(Current is TerminateProcessMessage);
        }

        public void Reset()
        {

        }

        public object Current
        {
            get; private set;
        }
    }
}