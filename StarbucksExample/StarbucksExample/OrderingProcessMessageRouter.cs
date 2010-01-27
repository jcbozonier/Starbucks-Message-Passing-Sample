﻿using System;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample
{
    public class OrderingProcessMessageRouter
    {
        private bool _Done;
        private IQueue _IncomingBaristaMessages;
        private IQueue _IncomingCustomerMessages;
        private IQueue _IncomingRegisterMessages;
        private IQueue _IncomingControllerMessages;

        private IChannel _OutgoingBaristaMessages;
        private IChannel _OutgoingCustomerMessages;
        private IChannel _OutgoingRegisterMessages;
        private IChannel _abandonedMessagesChannel;

        public void Process()
        {
            while(!_Done)
            {
                var incomingControllerMessage = _IncomingControllerMessages.Dequeue();
                var incomingBaristaMessage = _IncomingBaristaMessages.Dequeue();
                var incomingCustomerMessage = _IncomingCustomerMessages.Dequeue();
                var incomingRegisterMessage = _IncomingRegisterMessages.Dequeue();

                var outgoingBaristaMessage = _GetAppropriateOutboundChannelForBarista(incomingBaristaMessage);
                var outgoingCustomerMessage = _GetAppropriateOutboundChannelForCustomer(incomingCustomerMessage);
                var appropriateOutboundChannelForRegister = _GetAppropriateOutboundChannelForRegister(incomingRegisterMessage);

                _OutgoingBaristaMessages.Enqueue(outgoingBaristaMessage);
                _OutgoingCustomerMessages.Enqueue(outgoingCustomerMessage);
                appropriateOutboundChannelForRegister.Enqueue(incomingRegisterMessage);

                if (incomingControllerMessage is TerminateProcessMessage)
                {
                    _Done = true;
                    _OutgoingBaristaMessages.Enqueue(incomingControllerMessage);
                    _OutgoingCustomerMessages.Enqueue(incomingControllerMessage);
                    _OutgoingRegisterMessages.Enqueue(incomingControllerMessage);
                }
            }
        }

        private IChannel _GetAppropriateOutboundChannelForRegister(object incomingRegisterMessage)
        {
            if (incomingRegisterMessage is DrinkOrderRequestMessage ||
                incomingRegisterMessage is PaymentRequestMessage)
                return _OutgoingCustomerMessages;

            return _abandonedMessagesChannel;
        }

        private IChannel _GetAppropriateOutboundChannelForCustomer(object incomingCustomerMessage)
        {
            if (incomingCustomerMessage is DrinkRequestMessage ||
                incomingCustomerMessage is PaymentResponseMessage)
                return _OutgoingRegisterMessages;

            return _abandonedMessagesChannel;
        }

        private IChannel _GetAppropriateOutboundChannelForBarista(object incomingBaristaMessage)
        {
            if (incomingBaristaMessage is DrinkResponseMessage)
                return _OutgoingCustomerMessages;

            return _abandonedMessagesChannel;
        }
    }
}
