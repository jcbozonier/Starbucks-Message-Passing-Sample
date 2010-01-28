using System;

namespace StarbucksExample.MessagingSystem
{
    public interface IPeekableChannel : IChannel
    {
        bool IsEmpty();
    }
}
