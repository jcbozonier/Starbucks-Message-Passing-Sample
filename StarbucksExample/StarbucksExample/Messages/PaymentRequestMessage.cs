using System;

namespace StarbucksExample.Messages
{
    public class PaymentRequestMessage
    {
        private PaymentRequestMessage(string registerId, string customerId, decimal paymentAmountRequested)
        {
            RegisterId = registerId;
            CustomerId = customerId;
            PaymentAmountRequested = paymentAmountRequested;
        }

        public decimal PaymentAmountRequested { get; private set; }
        public string RegisterId { get; private set; }
        public string CustomerId { get; set; }

        public static PaymentRequestMessage Create(string registerId, string recipientId, decimal paymentAmountRequested)
        {
            return new PaymentRequestMessage(registerId, recipientId, paymentAmountRequested);
        }
    }
}