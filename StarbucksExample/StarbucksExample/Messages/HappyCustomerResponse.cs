namespace StarbucksExample.Messages
{
    public class HappyCustomerResponse : IMessage
    {
        public string CustomerId { get; private set; }

        public HappyCustomerResponse(string customerId)
        {
            CustomerId = customerId;
        }
    }
}