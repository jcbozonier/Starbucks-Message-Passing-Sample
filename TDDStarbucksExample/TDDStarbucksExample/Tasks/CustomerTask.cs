using System.Linq;
using TDDStarbucksExample.Messages;

namespace TDDStarbucksExample.Tasks
{
    public class CustomerTask
    {
        private readonly ISendChannel _CustomerProcessorOutboundChannel;
        private readonly int _NumberOfDrinkRequests;

        public CustomerTask(ISendChannel customerProcessorOutboundChannel, int numberOfDrinkRequests)
        {
            _CustomerProcessorOutboundChannel = customerProcessorOutboundChannel;
            _NumberOfDrinkRequests = numberOfDrinkRequests;
        }

        public void Process()
        {
            foreach (var i in Enumerable.Range(0, _NumberOfDrinkRequests))
                _CustomerProcessorOutboundChannel.Enqueue(new CustomerDrinkRequestMessage());
        }
    }
}


