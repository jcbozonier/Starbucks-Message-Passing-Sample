using System;

namespace StarbucksExample.Messages
{
    public class PaymentRequestMessage
    {
        public readonly DrinkRequestMessage CustomerOrder;

        private PaymentRequestMessage(string registerId, string customerId, decimal paymentAmountRequested, DrinkRequestMessage customerOrder)
        {
            CustomerOrder = customerOrder;
            RegisterId = registerId;
            CustomerId = customerId;
            PaymentAmountRequested = paymentAmountRequested;
        }

        public decimal PaymentAmountRequested { get; private set; }
        public string RegisterId { get; private set; }
        public string CustomerId { get; set; }

        public static PaymentRequestMessage Create(string registerId, string recipientId, decimal paymentAmountRequested, DrinkRequestMessage customerResponse)
        {
            return new PaymentRequestMessage(registerId, recipientId, paymentAmountRequested, customerResponse);
        }
    }
}