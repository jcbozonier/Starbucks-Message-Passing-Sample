namespace StarbucksExample.Messages
{
    public class DrinkRequestMessage
    {
        public string OriginationId { get; private set; }
        public string Size { get; private set; }
        public string DrinkDescription { get; private set; }

        private DrinkRequestMessage(string originationId, string size, string drinkDescription)
        {
            OriginationId = originationId;
            Size = size;
            DrinkDescription = drinkDescription;
        }

        public static DrinkRequestMessage Create(string originationId, string size, string drinkDescription)
        {
            return new DrinkRequestMessage(originationId, size, drinkDescription);
        }
    }
}