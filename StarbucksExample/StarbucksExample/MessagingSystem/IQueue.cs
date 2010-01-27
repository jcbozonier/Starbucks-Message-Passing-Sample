namespace StarbucksExample.MessagingSystem
{
    public interface IQueue : IChannel
    {
        bool IsEmpty();
    }
}
