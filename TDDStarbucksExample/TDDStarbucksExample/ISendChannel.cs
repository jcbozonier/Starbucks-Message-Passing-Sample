namespace TDDStarbucksExample
{
    public interface ISendChannel
    {
        void Enqueue(IMessage message);
    }
}