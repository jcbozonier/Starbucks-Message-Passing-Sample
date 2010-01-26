using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class CustomerActor
    {
        private readonly IQueue _RequestChannel;
        private readonly IQueue _ResponseChannel;
        public readonly string OriginationId;

        public CustomerActor(IQueue requestChannel, IQueue responseChannel)
        {
            OriginationId = new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            _RequestChannel.Enqueue(DrinkRequestMessage.Create(OriginationId, "Tall", "Half-Caf Double Decaf"));
            var registerResponse = _ResponseChannel.Dequeue() as PaymentRequestMessage;
            _RequestChannel.Enqueue(PaymentResponseMessage.Create(OriginationId, registerResponse.OriginationId, registerResponse.PaymentAmountRequested));
            var myDrink = _ResponseChannel.Dequeue() as DrinkResponseMessage;
            _RequestChannel.Enqueue(new HappyCustomerResponse(OriginationId));
        }
    }
}


