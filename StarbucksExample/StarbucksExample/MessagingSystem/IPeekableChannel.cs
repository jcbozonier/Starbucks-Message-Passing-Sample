using System;

namespace StarbucksExample.MessagingSystem
{
    public interface IPeekableChannel : IEnqueue, IDequeue
    {
        bool IsEmpty();
    }
}
