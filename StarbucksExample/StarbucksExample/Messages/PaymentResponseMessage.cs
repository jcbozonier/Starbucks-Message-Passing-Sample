namespace StarbucksExample.Messages
{
    public class PaymentResponseMessage
    {
        public string RegisterId { get; private set; }
        public string CustomerId { get; private set; }
        public decimal PaymentAmountProvided { get; private set; }

        private PaymentResponseMessage(string registerId, string customerId, decimal paymentAmountProvided)
        {
            RegisterId = registerId;
            CustomerId = customerId;
            PaymentAmountProvided = paymentAmountProvided;
        }

        public static PaymentResponseMessage Create(string registerId, string customerId, decimal paymentAmountProvided)
        {
            return new PaymentResponseMessage(registerId, customerId, paymentAmountProvided);
        }
    }
}