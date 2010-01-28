namespace StarbucksExample.MessagingSystem
{
    public interface IChannel
    {
        void Enqueue(object o);
        object Dequeue();
    }
}
