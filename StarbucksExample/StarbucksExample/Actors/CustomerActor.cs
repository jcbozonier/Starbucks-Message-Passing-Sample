using System;
using System.Linq;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class CustomerActor
    {
        private readonly IChannel _RequestChannel;
        private readonly IChannel _ResponseChannel;
        public readonly string OriginationId;
        private bool _Done;

        public CustomerActor(IChannel requestChannel, IChannel responseChannel)
        {
            OriginationId = new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            _SendOutDrinkRequests();

            while (!_Done)
            {
                var incomingMessage = _ResponseChannel.Dequeue();
                var outgoingMessage = _ProcessMessage(incomingMessage);

                if (outgoingMessage != null)
                    _ResponseChannel.Enqueue(outgoingMessage);
            }
        }

        private void _SendOutDrinkRequests()
        {
            var drinkRequests = Enumerable.Range(0, 10000).Select(
                x => DrinkRequestMessage.Create(x.ToString(), "Tall", "Half-Caf Double Decaf")).ToArray();

            foreach(var drinkRequest in drinkRequests)
                _RequestChannel.Enqueue(drinkRequest);
        }

        private object _ProcessMessage(object incomingMessage)
        {
            var registerResponse = incomingMessage as PaymentRequestMessage;
            var myDrink = incomingMessage as DrinkResponseMessage;
            var terminateProcessRequest = incomingMessage as TerminateProcessMessage;

            if(registerResponse != null)
                return PaymentResponseMessage.Create(registerResponse.CustomerId, registerResponse.RegisterId, registerResponse.PaymentAmountRequested);
            
            if(myDrink != null)
                return new HappyCustomerResponse(myDrink.CustomerId);

            if (terminateProcessRequest != null)
                _Done = true;

            return null;
        }
    }
}


