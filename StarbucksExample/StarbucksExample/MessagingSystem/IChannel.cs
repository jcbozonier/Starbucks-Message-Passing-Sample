
using StarbucksExample.Messages;

namespace StarbucksExample.MessagingSystem
{
    public interface IEnqueue {
        void Enqueue(IMessage o);
    }

    public interface IDequeue
    {
         IMessage Dequeue();
    }
}
