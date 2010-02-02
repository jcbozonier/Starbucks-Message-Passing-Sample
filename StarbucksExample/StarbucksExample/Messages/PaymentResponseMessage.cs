namespace StarbucksExample.Messages
{
    public class PaymentResponseMessage : IMessage
    {
        public string RegisterId { get; private set; }
        public string CustomerId { get; private set; }
        public decimal PaymentAmountProvided { get; private set; }
        public DrinkRequestMessage CustomerOrder { get; private set; }

        private PaymentResponseMessage(string registerId, string customerId, decimal paymentAmountProvided, DrinkRequestMessage customerOrder)
        {
            RegisterId = registerId;
            CustomerId = customerId;
            PaymentAmountProvided = paymentAmountProvided;
            CustomerOrder = customerOrder;
        }

        public static PaymentResponseMessage Create(string registerId, string customerId, decimal paymentAmountProvided, DrinkRequestMessage customerOrder)
        {
            return new PaymentResponseMessage(registerId, customerId, paymentAmountProvided, customerOrder);
        }
    }
}