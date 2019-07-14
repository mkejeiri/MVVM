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
				//property change notifications are essential to databinding because they notify the binding when their underlying data has changed so the binding can refresh
                //and keep the data in sync with the underlying DataModel
                //There are two options for raising property change notifications that a binding will automatically monitor
                //1- make the property as a DependencyProperty, which has its own internal notification mechanism that the binding is natively aware of,
                //DependencyProperties declarations are verbose, and require our object to inherit from dependency object,
                //both of which make it a heavyweight approach to achieve the goal of change notifications in model and view model objects
                //2- use INotifyPropertyChange (INPC) : more suitable for view and view model

                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(NotificationMessage)));
                }
             
            }
        }
		//a delegate trick where we assign an empty anonymous method in as a subscriber.
        //i.e. a subscriber is always in the list,so never we will have PropertyChanged being null.
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
