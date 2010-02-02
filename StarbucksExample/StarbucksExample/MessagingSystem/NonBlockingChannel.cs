using System;
using System.Collections;
using StarbucksExample.Messages;

namespace StarbucksExample.MessagingSystem
{
    public class NonBlockingChannel : IPeekableChannel
    {
        private readonly Queue _Queue;

        public NonBlockingChannel()
        {
            _Queue = Queue.Synchronized(new Queue());
        }

        public void Enqueue(IMessage o)
        {
            _Queue.Enqueue(o);
        }

        public IMessage Dequeue()
        {
            return (IMessage)_Queue.Dequeue();
        }

        public bool IsEmpty()
        {
            return _Queue.Count == 0;
        }
    }
}
