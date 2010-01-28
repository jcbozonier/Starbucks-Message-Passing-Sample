using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample
{
    public class OrderingProcessMessageRouter
    {
        private bool _Done;
        private readonly IPeekableChannel _IncomingMessages;
        private readonly IChannel _OutgoingBaristaMessages;
        private readonly IChannel _OutgoingCustomerMessages;
        private readonly IChannel _OutgoingRegisterMessages;
        private readonly IChannel _AbandonedMessagesChannel;

        public OrderingProcessMessageRouter(IPeekableChannel incomingMessages,
                                            IChannel outgoingBaristaMessages,
                                            IChannel outgoingCustomerMessages,
                                            IChannel outgoingRegisterMessages,
                                            IChannel abandonedMessagesChannel)
        {
            _AbandonedMessagesChannel = abandonedMessagesChannel;
            _OutgoingRegisterMessages = outgoingRegisterMessages;
            _OutgoingCustomerMessages = outgoingCustomerMessages;
            _IncomingMessages = incomingMessages;
            _OutgoingBaristaMessages = outgoingBaristaMessages;
        }

        public void Process()
        {
            while(!_Done)
            {
                if(!_IncomingMessages.IsEmpty())
                {
                    var message = _IncomingMessages.Dequeue();
                    var messageChannel = _GetAppropriateChannelFor(message);
                    messageChannel.Enqueue(message);
                }
            }
        }

        private IChannel _GetAppropriateChannelFor(object incomingMessage)
        {
            if (incomingMessage is PaymentRequestMessage)
                return _OutgoingCustomerMessages;
            if (incomingMessage is DrinkOrderRequestMessage)
                return _OutgoingBaristaMessages;
            if (incomingMessage is DrinkRequestMessage ||
                incomingMessage is PaymentResponseMessage)
                return _OutgoingRegisterMessages;
            if (incomingMessage is DrinkResponseMessage)
                return _OutgoingCustomerMessages;
            if (incomingMessage is HappyCustomerResponse)
            {
                Console.WriteLine("Another Happy Customer");
            }

            return _AbandonedMessagesChannel;
        }
    }
}
