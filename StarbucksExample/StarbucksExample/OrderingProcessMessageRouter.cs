using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample
{
    public class OrderingProcessMessageRouter
    {
        private bool _Done;
        private readonly IPeekableChannel _IncomingMessages;
        private readonly IEnqueue _OutgoingBaristaMessages;
        private readonly IEnqueue _OutgoingCustomerMessages;
        private readonly IEnqueue _OutgoingRegisterMessages;
        private readonly IEnqueue _AbandonedMessagesChannel;

        public OrderingProcessMessageRouter(IPeekableChannel incomingMessages,
                                            IEnqueue outgoingBaristaMessages,
                                            IEnqueue outgoingCustomerMessages,
                                            IEnqueue outgoingRegisterMessages,
                                            IEnqueue abandonedMessagesChannel)
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

        private IEnqueue _GetAppropriateChannelFor(object incomingMessage)
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
