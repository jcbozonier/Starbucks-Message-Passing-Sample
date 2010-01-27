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
            var baristaInboundChannel = new MyQueue();
            var baristaOutboundChannel = new ThreadSafeChannel();

            var customerInboundChannel = new MyQueue();
            var customerOutboundChannel = new ThreadSafeChannel();

            var registerInboundChannel = new MyQueue();
            var registerOutboundChannel = new ThreadSafeChannel();

            IQueue controllerInboundChannel = new MyQueue();
            IQueue controllerOutboundChannel = new MyQueue();
            IChannel abandonedMessages = new MyQueue();

            var messageRouter = new OrderingProcessMessageRouter(baristaInboundChannel, baristaOutboundChannel,
                                                          customerInboundChannel, customerOutboundChannel,
                                                          registerInboundChannel, registerOutboundChannel,
                                                          controllerInboundChannel, controllerOutboundChannel,
                                                          abandonedMessages);

            var baristaActor = new BaristaActor(baristaInboundChannel, baristaOutboundChannel);
            var customerActor = new CustomerActor(customerInboundChannel, customerOutboundChannel);
            var registerActor = new RegisterActor(registerInboundChannel, registerOutboundChannel);

            ThreadPool.QueueUserWorkItem(x => messageRouter.Process());
            ThreadPool.QueueUserWorkItem(x => baristaActor.Process());
            ThreadPool.QueueUserWorkItem(x => customerActor.Process());
            ThreadPool.QueueUserWorkItem(x => registerActor.Process());

            Console.ReadLine();
        }
    }
}
