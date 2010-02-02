using System.Collections;
using System.Collections.Generic;
using System.Threading;
using StarbucksExample.Messages;

namespace StarbucksExample.MessagingSystem
{
    public class EnumerableChannel<T> : IEnqueue, IEnumerable<T>, IEnumerator<T>
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

        T _Dequeue()
        {
            newItemEntered.WaitOne();

            lock (this)
            {
                var result = q.Dequeue();
                if (q.Count == 0)
                    newItemEntered.Reset();

                return (T)result;
            }

        }

        public IEnumerator<T> GetEnumerator()
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

        object IEnumerator.Current { get { return Current; } }

        public T Current
        {
            get; private set;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            
        }
    }
}