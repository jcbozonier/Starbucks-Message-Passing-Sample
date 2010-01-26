namespace StarbucksExample.Messages
{
    public class PaymentResponseMessage
    {
        public string OriginatorId { get; private set; }
        public string RecipientId { get; private set; }
        public decimal PaymentAmountProvided { get; private set; }

        private PaymentResponseMessage(string originatorId, string recipientId, decimal paymentAmountProvided)
        {
            OriginatorId = originatorId;
            RecipientId = recipientId;
            PaymentAmountProvided = paymentAmountProvided;
        }

        public static PaymentResponseMessage Create(string originatorId, string recipientId, decimal paymentAmountProvided)
        {
            return new PaymentResponseMessage(originatorId, recipientId, paymentAmountProvided);
        }
    }
}