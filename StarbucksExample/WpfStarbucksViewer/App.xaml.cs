using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using StarbucksExample;
using StarbucksExample.Actors;
using StarbucksExample.Messages;
using StarbucksExample.MessagingSystem;
using WpfStarbucksViewer.Tasks;
using WpfStarbucksViewer.ViewModels;

namespace WpfStarbucksViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += App_Startup;
        }

        void App_Startup(object sender, StartupEventArgs e)
        {

            var viewModel = new ProcessView();
            var statusChannel = new NonBlockingChannel();
            var uiTask = new ProcessViewUpdateTask(viewModel, statusChannel, Dispatcher);

            var viewer = new Window1();
            viewer.DataContext = viewModel;
            viewer.Show();

            var orchestration = new StarbucksOrchestration();
            orchestration.Process(statusChannel, uiTask);
        }
    }
}
