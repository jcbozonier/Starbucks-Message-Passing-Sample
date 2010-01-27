using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class BaristaActor
    {
        private readonly IChannel _RequestChannel;
        private readonly IChannel _ResponseChannel;
        public readonly string OriginationId;
        private bool _Done;

        public BaristaActor(IChannel requestChannel, IChannel responseChannel)
        {
            OriginationId = new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            while (!_Done)
            {
                var incomingMessage = _ResponseChannel.Dequeue();
                var drinkOrderRequest = incomingMessage as DrinkOrderRequestMessage;
                var terminationRequest = incomingMessage as TerminateProcessMessage;

                if(drinkOrderRequest != null)
                    _RequestChannel.Enqueue(DrinkResponseMessage.Create(OriginationId, drinkOrderRequest.RecipientId,
                                                                    drinkOrderRequest.Size,
                                                                    drinkOrderRequest.DrinkDescription));

                if(terminationRequest != null)
                    _Done = true;
            }
        }
    }
}


