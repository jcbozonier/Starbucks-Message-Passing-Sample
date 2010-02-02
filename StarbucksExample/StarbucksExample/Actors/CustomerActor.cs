using System;
using System.Collections;
using System.Linq;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class CustomerActor
    {
        private readonly IEnqueue _RequestChannel;
        private readonly IEnumerable _ResponseChannel;

        public CustomerActor(IEnqueue requestChannel, IEnumerable responseChannel)
        {
            new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            _SendOutDrinkRequests();

            foreach(var incomingMessage in _ResponseChannel)
            {
                var outgoingMessage = _ProcessMessage(incomingMessage);

                if (outgoingMessage != null)
                    _RequestChannel.Enqueue(outgoingMessage);
            }
        }

        private void _SendOutDrinkRequests()
        {
            var drinkRequests = Enumerable.Range(0, 100000).Select(
                x => DrinkRequestMessage.Create(x.ToString(), "Tall", "Half-Caf Double Decaf")).ToArray();

            foreach(var drinkRequest in drinkRequests)
                _RequestChannel.Enqueue(drinkRequest);
        }

        private static IMessage _ProcessMessage(object incomingMessage)
        {
            var registerResponse = incomingMessage as PaymentRequestMessage;
            var myDrink = incomingMessage as DrinkResponseMessage;
            var terminateProcessRequest = incomingMessage as TerminateProcessMessage;

            if(registerResponse != null)
                return PaymentResponseMessage.Create(registerResponse.CustomerId, registerResponse.RegisterId, registerResponse.PaymentAmountRequested, registerResponse.CustomerOrder);
            
            if(myDrink != null)
                return new HappyCustomerResponse(myDrink.CustomerId);

            if (incomingMessage == null)
                throw new InvalidOperationException("Received null message and this is invalid!");

            throw new InvalidOperationException("Received an incorrect response type for this actor.");
        }
    }
}


