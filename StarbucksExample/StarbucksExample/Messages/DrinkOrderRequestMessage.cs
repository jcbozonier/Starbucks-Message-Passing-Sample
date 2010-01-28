using System;

namespace StarbucksExample.Messages
{
    public class DrinkOrderRequestMessage
    {
        public string RegisterId { get; private set; }
        public string Size { get; private set; }
        public string DrinkDescription { get; private set; }
        public string CustomerId { get; private set; }

        private DrinkOrderRequestMessage(string registerId, string customerId, string size, string drinkDescription)
        {
            RegisterId = registerId;
            CustomerId = customerId;
            Size = size;
            DrinkDescription = drinkDescription;
        }

        public static DrinkOrderRequestMessage Create(string registerId, string customerId, string size, string drinkDescription)
        {
            return new DrinkOrderRequestMessage(registerId, customerId, size, drinkDescription);
        }
    }
}