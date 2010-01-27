using System.Collections;

namespace StarbucksExample.MessagingSystem
{
    public class MyQueue : IQueue
    {
        private readonly Queue _Queue;

        public MyQueue()
        {
            _Queue = new Queue();
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
