using System;
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
            _RequestChannel.Enqueue(DrinkRequestMessage.Create(OriginationId, "Tall", "Half-Caf Double Decaf"));

            while (!_Done)
            {
                var incomingMessage = _ResponseChannel.Dequeue();
                var outgoingMessage = _ProcessMessage(incomingMessage);

                if (outgoingMessage != null)
                    _ResponseChannel.Enqueue(outgoingMessage);
            }
        }

        private object _ProcessMessage(object incomingMessage)
        {
            var registerResponse = incomingMessage as PaymentRequestMessage;
            var myDrink = incomingMessage as DrinkResponseMessage;
            var terminateProcessRequest = incomingMessage as TerminateProcessMessage;

            if(registerResponse != null)
                return PaymentResponseMessage.Create(OriginationId, registerResponse.OriginationId, registerResponse.PaymentAmountRequested);
            
            if(myDrink != null)
                return new HappyCustomerResponse(OriginationId);

            if (terminateProcessRequest != null)
                _Done = true;

            return null;
        }
    }
}


