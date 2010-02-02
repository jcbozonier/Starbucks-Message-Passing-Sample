using System;
using NUnit.Framework;
using TDDStarbucksExample;
using TDDStarbucksExample.Messages;
using TDDStarbucksExample.Tasks;

namespace Given_a_register_task.and_an_outbound_channel
{
    public class When_a_register_task_receives_a_customer_drink_order_request
    {
        private FakeChannel It;

        [Test]
        public void It_should_place_a_register_payment_request_in_its_outbound_channel()
        {
            Assert.That(It.ReceivedMessage, Is.TypeOf<RegisterPaymentRequestMessage>());
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            var registerIncomingChannel = new FakeChannel();
            registerIncomingChannel.Enqueue(new CustomerDrinkRequestMessage());

            It = new FakeChannel();
            var registerTask = new RegisterTask(It, registerIncomingChannel);

            registerTask.Process();
        }

        

        public class FakeChannel : ISendChannel, IReceiveChannel
        {
            public RegisterPaymentRequestMessage ReceivedMessage;

            public void Enqueue(IMessage message)
            {
                ReceivedMessage = message as RegisterPaymentRequestMessage;
            }

            public IMessage Dequeue()
            {
                return ReceivedMessage;   
            }

            public bool IsEmpty()
            {
                return false;
            }
        }
    }

    public class When_a_register_task_begins_processing_with_no_messages
    {
        private FakeChannel It;
        private Exception ActualException;

        [Test]
        public void It_should_asplode()
        {
            Assert.That(ActualException, Is.Not.Null);
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            It = new FakeChannel();
            var registerTask = new RegisterTask(It, new FakeChannel());

            try
            {
                registerTask.Process();
            }
            catch(Exception err)
            {
                ActualException = err;
            }
        }

        public class FakeChannel : ISendChannel, IReceiveChannel
        {
            public bool ReceivedAMessage;

            public void Enqueue(IMessage message)
            {
                ReceivedAMessage = true;
            }

            public IMessage Dequeue()
            {
                throw new NotSupportedException();
            }

            public bool IsEmpty()
            {
                return true;
            }
        }
    }
}
