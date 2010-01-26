using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class BaristaActor
    {
        private readonly IQueue _RequestChannel;
        private readonly IQueue _ResponseChannel;
        public readonly string OriginationId;

        public BaristaActor(IQueue requestChannel, IQueue responseChannel)
        {
            OriginationId = new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            var drinkOrderRequest = _ResponseChannel.Dequeue() as DrinkOrderRequestMessage;
            _RequestChannel.Enqueue(DrinkResponseMessage.Create(OriginationId, drinkOrderRequest.RecipientId, drinkOrderRequest.Size, drinkOrderRequest.DrinkDescription));
        }
    }
}


