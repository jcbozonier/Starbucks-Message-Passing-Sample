namespace StarbucksExample.Messages
{
    public class HappyCustomerResponse
    {
        public string CustomerId { get; private set; }

        public HappyCustomerResponse(string customerId)
        {
            CustomerId = customerId;
        }
    }
}