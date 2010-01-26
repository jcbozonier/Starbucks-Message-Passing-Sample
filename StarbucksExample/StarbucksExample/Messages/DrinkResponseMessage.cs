namespace StarbucksExample.Messages
{
    public class DrinkResponseMessage
    {
        public string OriginationId { get; private set; }
        public string RecipientId { get; private set; }
        public string Size { get; private set; }
        public string DrinkDescription { get; private set; }

        private DrinkResponseMessage(string originationId, string recipientId, string size, string drinkDescription)
        {
            OriginationId = originationId;
            RecipientId = recipientId;
            Size = size;
            DrinkDescription = drinkDescription;
        }

        public static DrinkResponseMessage Create(string originationId, string recipientId, string size, string drinkDescription)
        {
            return new DrinkResponseMessage(originationId, recipientId, size, drinkDescription);
        }
    }
}