using System;
using System.Threading;
using StarbucksExample.Actors;
using StarbucksExample.MessagingSystem;

namespace StarbucksExample
{
    public class StarbucksOrchestration
    {
        public void Process()
        {
            var inboundChannel = new MyPeekableChannel();

            var baristaOutboundChannel = new ThreadSafeChannel();
            var customerOutboundChannel = new ThreadSafeChannel();
            var registerOutboundChannel = new ThreadSafeChannel();

            IPeekableChannel controllerInboundChannel = new MyPeekableChannel();
            IPeekableChannel controllerOutboundChannel = new MyPeekableChannel();
            IChannel abandonedMessages = new MyPeekableChannel();

            var messageRouter = new OrderingProcessMessageRouter(inboundChannel, baristaOutboundChannel,
                                                          customerOutboundChannel, registerOutboundChannel,
                                                          controllerInboundChannel, controllerOutboundChannel,
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
