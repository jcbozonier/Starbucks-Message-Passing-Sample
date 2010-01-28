using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class RegisterActor
    {
        private readonly IChannel _RequestChannel;
        private readonly IChannel _ResponseChannel;
        public readonly string RegisterId;
        private bool _Done;

        public RegisterActor(IChannel requestChannel, IChannel responseChannel)
        {
            RegisterId = new Guid().ToString();
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
                return PaymentRequestMessage.Create(RegisterId, customerResponse.CustomerId, (decimal)2.15, customerResponse);
            
            if(paymentResponse != null)
                return DrinkOrderRequestMessage.Create(RegisterId, paymentResponse.CustomerId, paymentResponse.CustomerOrder.Size, paymentResponse.CustomerOrder.DrinkDescription);

            if (terminateProcessRequest != null)
                _Done = true;

            if (incomingMessage == null)
                throw new InvalidOperationException("Received null message and this is invalid!");

            throw new InvalidOperationException("Received an incorrect response type for this actor.");
        }
    }
}


