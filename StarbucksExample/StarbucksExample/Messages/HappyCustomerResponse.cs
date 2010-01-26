namespace StarbucksExample.Messages
{
    public class HappyCustomerResponse
    {
        public string OriginationId { get; private set; }

        public HappyCustomerResponse(string originationId)
        {
            OriginationId = originationId;
        }
    }
}