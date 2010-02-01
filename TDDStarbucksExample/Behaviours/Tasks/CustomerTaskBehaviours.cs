using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TDDStarbucksExample;
using TDDStarbucksExample.Messages;
using TDDStarbucksExample.Tasks;

namespace Behaviours.Given_a_customer_task.and_an_outbound_channel
{
    [TestFixture]
    public class When_a_customer_task_begins_processing_with_an_outbound_channel
    {
        private FakeChannel It;

        [Test]
        public void It_should_send_a_customer_order_message_on_its_outbound_channel()
        {
            Assert.That(It.ReceivedMessage, Is.TypeOf<CustomerDrinkRequestMessage>());
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            It = new FakeChannel();
            var customerProcessor = new CustomerTask(It, 1);

            customerProcessor.Process();
        }
    }

    public class FakeChannel : ISendChannel
    {
        public IMessage ReceivedMessage;

        public void Enqueue(IMessage message)
        {
            ReceivedMessage = message;
        }
    }
}

namespace Behaviours.Given_a_customer_task.and_an_outbound_channel.and_a_drink_order_request_count
{
    [TestFixture]
    public class When_a_customer_processor_begins_processing_with_an_outbound_channel
    {
        private FakeChannel It;
        private int ExpectedNumberOfDrinkRequests;

        [Test]
        public void It_should_send_the_correct_number_of_customer_order_messages_on_its_outbound_channel()
        {
            Assert.That(It.ReceivedMessages.Count(), Is.EqualTo(ExpectedNumberOfDrinkRequests));
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            It = new FakeChannel();
            ExpectedNumberOfDrinkRequests = 100;
            var customerProcessor = new CustomerTask(It, ExpectedNumberOfDrinkRequests);

            customerProcessor.Process();
        }
    }

    public class FakeChannel : ISendChannel
    {
        public readonly List<IMessage> ReceivedMessages;

        public FakeChannel()
        {
            ReceivedMessages = new List<IMessage>();
        }

        public void Enqueue(IMessage message)
        {
            ReceivedMessages.Add(message);
        }
    }
}
