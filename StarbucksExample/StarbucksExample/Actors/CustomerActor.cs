using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class CustomerActor
    {
        private readonly IEnqueue _RequestChannel;
        private readonly IEnumerable<IMessage> _ResponseChannel;

        public CustomerActor(IEnqueue requestChannel, IEnumerable<IMessage> responseChannel)
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
            int ordersToProcess = 14;
            var drinkRequests = Enumerable.Range(0, ordersToProcess).Select(
                x => DrinkRequestMessage.Create(x.ToString(), "Tall", "Half-Caf Double Decaf")).ToArray();

            foreach(var drinkRequest in drinkRequests)
            {
                _RequestChannel.Enqueue(drinkRequest);
            }
        }

        private static IMessage _ProcessMessage(object incomingMessage)
        {
            var registerResponse = incomingMessage as PaymentRequestMessage;
            var myDrink = incomingMessage as DrinkResponseMessage;

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


