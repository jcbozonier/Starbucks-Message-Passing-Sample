using NUnit.Framework;
using TDDStarbucksExample;

namespace Given_a_register_task.and_an_outbound_channel
{
    public class When_a_register_task_begins_processing_with_an_outbound_channel
    {
        private FakeChannel It;

        [Test]
        public void It_should_place_nothing_in_its_outbound_channel()
        {
            Assert.That(It.ReceivedAMessage, Is.False);
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            It = new FakeChannel();
            var registerTask = new RegisterTask(It);

            registerTask.Process();
        }
    }

    public class FakeChannel : ISendChannel
    {
        public bool ReceivedAMessage;

        public void Enqueue(IMessage message)
        {
            ReceivedAMessage = true;
        }
    }

    public class RegisterTask
    {
        public RegisterTask(ISendChannel sendChannel)
        {
        }

        public void Process()
        {
        }
    }
}
