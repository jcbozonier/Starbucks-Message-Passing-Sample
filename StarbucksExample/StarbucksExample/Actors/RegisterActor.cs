using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class RegisterActor
    {
        private readonly IQueue _RequestChannel;
        private readonly IQueue _ResponseChannel;
        public readonly string OriginationId;
        private bool _Done;

        public RegisterActor(IQueue requestChannel, IQueue responseChannel)
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
                var outgoingMessage = _Process(incomingMessage);

                if (outgoingMessage != null)
                    _RequestChannel.Enqueue(outgoingMessage);
            }
        }

        private object _Process(object incomingMessage)
        {
            var customerResponse = incomingMessage as DrinkRequestMessage;
            var paymentResponse = incomingMessage as PaymentResponseMessage;
            var terminateProcessRequest = incomingMessage as TerminateProcessMessage;

            if(customerResponse != null)
                return PaymentRequestMessage.Create(OriginationId, customerResponse.OriginationId, (decimal)2.15);
            
            if(paymentResponse != null)
                return DrinkOrderRequestMessage.Create(OriginationId, customerResponse.OriginationId, customerResponse.Size, customerResponse.DrinkDescription);

            if (terminateProcessRequest != null)
                _Done = true;

            return null;
        }
    }
}


