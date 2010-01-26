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

        public RegisterActor(IQueue requestChannel, IQueue responseChannel)
        {
            OriginationId = new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            var customerResponse = _ResponseChannel.Dequeue() as DrinkRequestMessage;
            _RequestChannel.Enqueue(PaymentRequestMessage.Create(OriginationId, customerResponse.OriginationId, (decimal)2.15));
            var paymentResponse = _ResponseChannel.Dequeue() as PaymentResponseMessage;
            _RequestChannel.Enqueue(DrinkOrderRequestMessage.Create(OriginationId, customerResponse.OriginationId, customerResponse.Size, customerResponse.DrinkDescription));

        }
    }
}


