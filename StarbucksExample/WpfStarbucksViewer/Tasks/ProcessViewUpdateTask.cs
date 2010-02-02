using System;
using System.Threading;
using System.Windows.Threading;
using StarbucksExample.Actors;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;
using WpfStarbucksViewer.ViewModels;

namespace WpfStarbucksViewer.Tasks
{
    public class ProcessViewUpdateTask : ITaskable
    {
        readonly ProcessView _ViewModel;
        readonly NonBlockingChannel _StatusChannel;
        readonly Dispatcher _Dispatcher;

        public ProcessViewUpdateTask(ProcessView viewModel,
                                     NonBlockingChannel statusChannel,
                                     Dispatcher dispatcher)
        {
            _ViewModel = viewModel;
            _StatusChannel = statusChannel;
            _Dispatcher = dispatcher;
        }

        public void Process()
        {
            var done = false;
            while (!done)
            {
                if (!((IPeekableChannel)_StatusChannel).IsEmpty())
                {
                    var message = ((IPeekableChannel)_StatusChannel).Dequeue();

                    var isCustomerMessage = _IsCustomerMessage(message);
                    var isRegisterMessage = _IsRegisterMessage(message);
                    var isBaristaMessage = _IsBaristaMessage(message);
                    var isTerminateMessage = message is TerminateProcessMessage;

                    if (isTerminateMessage)
                    {
                        done = true;
                    }

                    _Dispatcher.Invoke((Action) (() =>
                                                 {
                                                     var statusMessage = String.Empty;

                                                     if (isCustomerMessage)
                                                     {
                                                         statusMessage = "CustomerTask in process...";
                                                         if (message is HappyCustomerResponse)
                                                             statusMessage = "CustomerTask says: I'm happy!";
                                                         if (message is DrinkRequestMessage)
                                                             statusMessage = "CustomerTask says: I'm thirsty!";

                                                         _ViewModel.CustomerStatusMessages.Add(statusMessage);
                                                     }
                                                     if (isRegisterMessage)
                                                     {
                                                         _ViewModel.RegisterStatusMessages.Add(
                                                                 "RegisterProcess sent message");
                                                     }
                                                     if (isBaristaMessage)
                                                     {
                                                         _ViewModel.BaristaStatusMessages.Add(
                                                                 "BaristaProcess sent message");
                                                     }
                                                 }),
                                       DispatcherPriority.Normal);
                }
            }
        }

        static bool _IsBaristaMessage(IMessage message)
        {
            return message is DrinkResponseMessage;
        }

        static bool _IsRegisterMessage(IMessage message)
        {
            return message is PaymentRequestMessage ||
                   message is DrinkOrderRequestMessage;
        }

        static bool _IsCustomerMessage(IMessage message)
        {
            return message is DrinkRequestMessage ||
                   message is PaymentResponseMessage ||
                   message is HappyCustomerResponse;
        }
    }
}
