using System;

namespace StarbucksExample.Messages
{
    public class DrinkOrderRequestMessage
    {
        public string OriginationId { get; private set; }
        public string Size { get; private set; }
        public string DrinkDescription { get; private set; }
        public string RecipientId { get; private set; }

        private DrinkOrderRequestMessage(string originationId, string recipientId, string size, string drinkDescription)
        {
            OriginationId = originationId;
            RecipientId = recipientId;
            Size = size;
            DrinkDescription = drinkDescription;
        }

        public static DrinkOrderRequestMessage Create(string originationId, string recipientId, string size, string drinkDescription)
        {
            return new DrinkOrderRequestMessage(originationId, recipientId, size, drinkDescription);
        }
    }
}