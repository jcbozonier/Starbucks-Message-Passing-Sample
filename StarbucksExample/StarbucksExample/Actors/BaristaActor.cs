using System;
using System.Collections.Generic;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class BaristaActor
    {
        private readonly IEnqueue _RequestChannel;
        private readonly IEnumerable<IMessage> _ResponseChannel;

        public BaristaActor(IEnqueue requestChannel, IEnumerable<IMessage> responseChannel)
        {
            new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            foreach(var incomingMessage in _ResponseChannel)
            {
                var drinkOrderRequest = incomingMessage as DrinkOrderRequestMessage;

                if(drinkOrderRequest != null)
                    _RequestChannel.Enqueue(DrinkResponseMessage.Create(drinkOrderRequest.CustomerId,
                                                                    drinkOrderRequest.Size,
                                                                    drinkOrderRequest.DrinkDescription));
            }
        }
    }
}


