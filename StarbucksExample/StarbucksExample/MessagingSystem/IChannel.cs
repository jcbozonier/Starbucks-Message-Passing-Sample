namespace StarbucksExample.MessagingSystem
{
    public interface IEnqueue {
        void Enqueue(object o);
    }

    public interface IDequeue {
        object Dequeue();
    }

    public interface IChannel : IEnqueue, IDequeue
    {}
}
