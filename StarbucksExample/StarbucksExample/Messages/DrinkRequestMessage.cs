namespace StarbucksExample.Messages
{
    public class DrinkRequestMessage : IMessage
    {
        public string CustomerId { get; private set; }
        public string Size { get; private set; }
        public string DrinkDescription { get; private set; }

        private DrinkRequestMessage(string originationId, string size, string drinkDescription)
        {
            CustomerId = originationId;
            Size = size;
            DrinkDescription = drinkDescription;
        }

        public static DrinkRequestMessage Create(string originationId, string size, string drinkDescription)
        {
            return new DrinkRequestMessage(originationId, size, drinkDescription);
        }
    }
}