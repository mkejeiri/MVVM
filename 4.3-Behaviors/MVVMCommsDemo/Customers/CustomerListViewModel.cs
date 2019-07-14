using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MVVMCommsDemo.Services;
using Zza.Data;

namespace MVVMCommsDemo.Customers
{
    public class CustomerListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _customers;
        private ICustomersRepository _repository = new CustomersRepository();

        public CustomerListViewModel()
        {
            DeleteCommand = new RelayCommand(OnDelete, CanDelete);
        }

        public async void LoadCustomers()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;
            Customers = new ObservableCollection<Customer>(await _repository.GetCustomersAsync());
        }
		/*
         * Commands on buttons have a command property to hook up to an ICommand exposed as a property on our view model that is an ICommand
         * and bind to it from the Button's Command property = > i.e. DeleteCommand property of type ICommand as the XAML gets parsed here :
         * 1 - it calls the get block on that ICommand property to get a reference to the Command object
         * 2-  it calls CanExecute on that command to determine the initial enabled or disabled state of the command, and it will enable or disable the button as a result.
         * 3-  it will subscribe to CanExecuteChanged on that ICommand, which allows it to be notified in the future if the enabled or disabled state of that command changes
         */

        //private setter because it's on set once inside the ctor
        public RelayCommand DeleteCommand { get; private set; }

        public ObservableCollection<Customer> Customers
        {
            get
            {
                return _customers;
            }
            set
            {
                if (_customers != value)
                {
                    _customers = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Customers)));
                }

            }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                //PropertyChanged is not necessary since the SelectedCustomer is set by the view,
                //so only change is done when selected item in the grid
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                    DeleteCommand.RaiseCanExecuteChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedCustomer)));
                }
            }
        }

        private void OnDelete()
        {
            Customers.Remove(SelectedCustomer);
        }

        private bool CanDelete()
        {
            return SelectedCustomer != null;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
