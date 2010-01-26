using System;

namespace StarbucksExample.Messages
{
    public class PaymentRequestMessage
    {
        private PaymentRequestMessage(string originationId, string recipientId, decimal paymentAmountRequested)
        {
            OriginationId = originationId;
            RecipientId = recipientId;
            PaymentAmountRequested = paymentAmountRequested;
        }

        public decimal PaymentAmountRequested { get; private set; }
        public string OriginationId { get; private set; }
        public string RecipientId { get; set; }

        public static PaymentRequestMessage Create(string originationId, string recipientId, decimal paymentAmountRequested)
        {
            return new PaymentRequestMessage(originationId, recipientId, paymentAmountRequested);
        }
    }
}