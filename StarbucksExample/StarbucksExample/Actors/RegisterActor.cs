
﻿using System;
using System.Collections;
using System.Collections.Generic;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample.Actors
{
    public class RegisterActor : ITaskable
    {
        private readonly IEnqueue _RequestChannel;
        private readonly IEnumerable<IMessage> _ResponseChannel;
        public readonly string RegisterId;

        public RegisterActor(IEnqueue requestChannel, IEnumerable<IMessage> responseChannel)
        {
            RegisterId = new Guid().ToString();
            _RequestChannel = requestChannel;
            _ResponseChannel = responseChannel;
        }

        public void Process()
        {
            foreach (var incomingMessage in _ResponseChannel)
            {
                var i = 100000;
                var b = 0;
                while(i-- > 0)
                {
                    b++;
                }

                var outgoingMessage = _Process(incomingMessage);

                if (outgoingMessage != null)
                    _RequestChannel.Enqueue(outgoingMessage);
            }
        }

        private IMessage _Process(object incomingMessage)
        {
            var customerResponse = incomingMessage as DrinkRequestMessage;
            var paymentResponse = incomingMessage as PaymentResponseMessage;

            if(customerResponse != null)
                return PaymentRequestMessage.Create(RegisterId, customerResponse.CustomerId, (decimal)2.15, customerResponse);
            
            if(paymentResponse != null)
                return DrinkOrderRequestMessage.Create(RegisterId, paymentResponse.CustomerId, paymentResponse.CustomerOrder.Size, paymentResponse.CustomerOrder.DrinkDescription);

            if (incomingMessage == null)
                throw new InvalidOperationException("Received null message and this is invalid!");

            throw new InvalidOperationException("Received an incorrect response type for this actor.");
        }
    }
}


