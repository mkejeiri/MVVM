using System;
using System.ComponentModel;
using System.Timers;
using MVVMCommsDemo.Customers;

namespace MVVMCommsDemo
{
    public class MainWindowViewModel:INotifyPropertyChanged
    {
        private string _notificationMessage;
        private Timer _timer = new Timer(5000); 
        public MainWindowViewModel()
        {
            CurrentViewModel = new CustomerListViewModel();
            _timer.Elapsed += (s, e) =>
                 NotificationMessage = $"At the tone the time will be: {DateTime.Now.ToLocalTime()}";
            _timer.Start();
        }
        public object CurrentViewModel { get; set; }

        public string NotificationMessage
        {
            get => _notificationMessage;
            set
            {
                if (_notificationMessage != value)
                {
                    _notificationMessage = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotificationMessage)));
                }
             
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
