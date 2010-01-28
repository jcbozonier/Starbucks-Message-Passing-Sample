using System;
using System.Collections;

namespace StarbucksExample.MessagingSystem
{
    public class NonBlockingChannel : IPeekableChannel
    {
        private readonly Queue _Queue;

        public NonBlockingChannel()
        {
            _Queue = Queue.Synchronized(new Queue());
        }

        public void Enqueue(object o)
        {
            _Queue.Enqueue(o);
        }

        public object Dequeue()
        {
            return _Queue.Dequeue();
        }

        public bool IsEmpty()
        {
            return _Queue.Count == 0;
        }
    }
}
