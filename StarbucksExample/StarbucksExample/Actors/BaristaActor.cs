using System;
using System.Collections;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class BaristaActor
    {
        private readonly IEnqueue _RequestChannel;
        private readonly IEnumerable _ResponseChannel;
        private bool _Done;

        public BaristaActor(IEnqueue requestChannel, IEnumerable responseChannel)
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
                var terminationRequest = incomingMessage as TerminateProcessMessage;

                if(drinkOrderRequest != null)
                    _RequestChannel.Enqueue(DrinkResponseMessage.Create(drinkOrderRequest.CustomerId,
                                                                    drinkOrderRequest.Size,
                                                                    drinkOrderRequest.DrinkDescription));

                if(terminationRequest != null)
                    _Done = true;
            }
        }
    }
}


