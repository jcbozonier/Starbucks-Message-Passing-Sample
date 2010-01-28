using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample
{
    public class OrderingProcessMessageRouter
    {
        private bool _Done;
        private readonly IPeekableChannel _IncomingMessages;
        private readonly IPeekableChannel _IncomingControllerMessages;
        private readonly IChannel _OutgoingBaristaMessages;
        private readonly IChannel _OutgoingCustomerMessages;
        private readonly IChannel _OutgoingRegisterMessages;
        private readonly IChannel _AbandonedMessagesChannel;
        private readonly IPeekableChannel _OutgoingControllerMessages;

        public OrderingProcessMessageRouter(IPeekableChannel incomingMessages,
                                            IChannel outgoingBaristaMessages,
                                            IChannel outgoingCustomerMessages,
                                            IChannel outgoingRegisterMessages,
                                            IPeekableChannel incomingControllerMessages,
                                            IPeekableChannel outgoingControllerMessages,
                                            IChannel abandonedMessagesChannel)
        {
            _AbandonedMessagesChannel = abandonedMessagesChannel;
            _OutgoingControllerMessages = outgoingControllerMessages;
            _IncomingControllerMessages = incomingControllerMessages;
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

                if (!_IncomingControllerMessages.IsEmpty())
                {
                    var incomingControllerMessage = _IncomingControllerMessages.Dequeue();
                    if (incomingControllerMessage is TerminateProcessMessage)
                    {
                        _Done = true;
                        _OutgoingBaristaMessages.Enqueue(incomingControllerMessage);
                        _OutgoingCustomerMessages.Enqueue(incomingControllerMessage);
                        _OutgoingRegisterMessages.Enqueue(incomingControllerMessage);
                    }
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
                return _OutgoingControllerMessages;
            }

            return _AbandonedMessagesChannel;
        }

        private IChannel _GetAppropriateOutboundChannelForRegister(object incomingRegisterMessage)
        {
            if (incomingRegisterMessage is PaymentRequestMessage)
                return _OutgoingCustomerMessages;
            if (incomingRegisterMessage is DrinkOrderRequestMessage)
                return _OutgoingBaristaMessages;

            return _AbandonedMessagesChannel;
        }

        private IChannel _GetAppropriateOutboundChannelForCustomer(object incomingCustomerMessage)
        {
            if (incomingCustomerMessage is DrinkRequestMessage ||
                incomingCustomerMessage is PaymentResponseMessage)
                return _OutgoingRegisterMessages;

            if (incomingCustomerMessage is HappyCustomerResponse)
            {
                Console.WriteLine("Another Happy Customer");
                return _OutgoingControllerMessages;
            }

            return _AbandonedMessagesChannel;
        }

        private IChannel _GetAppropriateOutboundChannelForBarista(object incomingBaristaMessage)
        {
            if (incomingBaristaMessage is DrinkResponseMessage)
                return _OutgoingCustomerMessages;

            return _AbandonedMessagesChannel;
        }
    }
}
