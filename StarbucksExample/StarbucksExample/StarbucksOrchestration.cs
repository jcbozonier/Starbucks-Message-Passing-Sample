﻿using System;
using System.Threading;
using StarbucksExample.Actors;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample
{
    public class StarbucksOrchestration
    {
        public void Process()
        {
            var inboundChannel = new NonBlockingChannel();
            var baristaOutboundChannel = new EnumerableChannel<IMessage>();
            var customerOutboundChannel = new EnumerableChannel<IMessage>();
            var registerOutboundChannel = new EnumerableChannel<IMessage>();
            var abandonedMessages = new NonBlockingChannel();

            var messageRouter = new OrderingProcessMessageRouter(
                inboundChannel, 
                baristaOutboundChannel,
                customerOutboundChannel, 
                registerOutboundChannel,
                abandonedMessages);

            var baristaActor = new BaristaActor(inboundChannel, baristaOutboundChannel);
            var customerActor = new CustomerActor(inboundChannel, customerOutboundChannel);
            var registerActor = new RegisterActor(inboundChannel, registerOutboundChannel);

            ThreadPool.QueueUserWorkItem(x => messageRouter.Process());
            ThreadPool.QueueUserWorkItem(x => baristaActor.Process());
            ThreadPool.QueueUserWorkItem(x => customerActor.Process());
            ThreadPool.QueueUserWorkItem(x => registerActor.Process());

            Console.ReadLine();
        }
    }
}
