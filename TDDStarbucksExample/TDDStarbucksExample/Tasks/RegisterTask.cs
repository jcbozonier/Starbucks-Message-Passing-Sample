using TDDStarbucksExample.Messages;

namespace TDDStarbucksExample.Tasks
{
    public class RegisterTask
    {
        private readonly ISendChannel _SendChannel;
        private readonly IReceiveChannel _RegisterIncomingChannel;

        public RegisterTask(ISendChannel sendChannel, IReceiveChannel registerIncomingChannel)
        {
            _SendChannel = sendChannel;
            _RegisterIncomingChannel = registerIncomingChannel;
        }

        public void Process()
        {
            var message = _RegisterIncomingChannel.Dequeue();
            
            _SendChannel.Enqueue(new RegisterPaymentRequestMessage());
        }
    }
}