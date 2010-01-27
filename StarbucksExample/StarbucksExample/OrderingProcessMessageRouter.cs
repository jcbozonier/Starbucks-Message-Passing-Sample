using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample
{
    public class OrderingProcessMessageRouter
    {
        private bool _Done;
        private readonly IQueue _IncomingBaristaMessages;
        private readonly IQueue _IncomingCustomerMessages;
        private readonly IQueue _IncomingRegisterMessages;
        private readonly IQueue _IncomingControllerMessages;

        private readonly IChannel _OutgoingBaristaMessages;
        private readonly IChannel _OutgoingCustomerMessages;
        private readonly IChannel _OutgoingRegisterMessages;
        private readonly IChannel _AbandonedMessagesChannel;
        private readonly IQueue _OutgoingControllerMessages;

        public OrderingProcessMessageRouter(
            IQueue incomingBaristaMessages, 
            IChannel outgoingBaristaMessages, 
            IQueue incomingCustomerMessages, 
            IChannel outgoingCustomerMessages, 
            IQueue incomingRegisterMessages, 
            IChannel outgoingRegisterMessages, 
            IQueue incomingControllerMessages, 
            IQueue outgoingControllerMessages, 
            IChannel abandonedMessagesChannel)
        {
            _IncomingBaristaMessages = incomingBaristaMessages;
            _AbandonedMessagesChannel = abandonedMessagesChannel;
            _OutgoingControllerMessages = outgoingControllerMessages;
            _IncomingControllerMessages = incomingControllerMessages;
            _OutgoingRegisterMessages = outgoingRegisterMessages;
            _IncomingRegisterMessages = incomingRegisterMessages;
            _OutgoingCustomerMessages = outgoingCustomerMessages;
            _IncomingCustomerMessages = incomingCustomerMessages;
            _OutgoingBaristaMessages = outgoingBaristaMessages;
        }

        public void Process()
        {
            while(!_Done)
            {
                if (!_IncomingBaristaMessages.IsEmpty())
                {
                    var incomingBaristaMessage = _IncomingBaristaMessages.Dequeue();
                    var appropriateOutboundChannelForBarista = _GetAppropriateOutboundChannelForBarista(incomingBaristaMessage);
                    appropriateOutboundChannelForBarista.Enqueue(incomingBaristaMessage);
                }

                if (!_IncomingCustomerMessages.IsEmpty())
                {
                    var incomingCustomerMessage = _IncomingCustomerMessages.Dequeue();
                    var appropriateOutboundChannelForCustomer = _GetAppropriateOutboundChannelForCustomer(incomingCustomerMessage);
                    appropriateOutboundChannelForCustomer.Enqueue(incomingCustomerMessage);
                }

                if (!_IncomingRegisterMessages.IsEmpty())
                {
                    var incomingRegisterMessage = _IncomingRegisterMessages.Dequeue();
                    var appropriateOutboundChannelForRegister = _GetAppropriateOutboundChannelForRegister(incomingRegisterMessage);
                    appropriateOutboundChannelForRegister.Enqueue(incomingRegisterMessage);
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
