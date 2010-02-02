using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfStarbucksViewer.ViewModels
{
    public class ProcessView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ProcessView()
        {
            CustomerStatusMessages = new ObservableCollection<string>();
            RegisterStatusMessages = new ObservableCollection<string>();
            BaristaStatusMessages = new ObservableCollection<string>();
        }

        public ObservableCollection<string> CustomerStatusMessages { get; private set; }
        public ObservableCollection<string> RegisterStatusMessages { get; private set; }
        public ObservableCollection<string> BaristaStatusMessages { get; private set; }
    }
}
