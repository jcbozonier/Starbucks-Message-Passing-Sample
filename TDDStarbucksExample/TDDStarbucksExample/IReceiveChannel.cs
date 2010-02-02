namespace TDDStarbucksExample
{
    public interface IReceiveChannel
    {
        IMessage Dequeue();
        bool IsEmpty();
    }
}
