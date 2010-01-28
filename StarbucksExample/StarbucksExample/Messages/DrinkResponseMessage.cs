namespace StarbucksExample.Messages
{
    public class DrinkResponseMessage
    {
        public string CustomerId { get; private set; }
        public string Size { get; private set; }
        public string DrinkDescription { get; private set; }

        private DrinkResponseMessage(string customerId, string size, string drinkDescription)
        {
            CustomerId = customerId;
            Size = size;
            DrinkDescription = drinkDescription;
        }

        public static DrinkResponseMessage Create(string customerId, string size, string drinkDescription)
        {
            return new DrinkResponseMessage(customerId, size, drinkDescription);
        }
    }
}