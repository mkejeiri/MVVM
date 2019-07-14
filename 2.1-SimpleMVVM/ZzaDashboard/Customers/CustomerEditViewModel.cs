using System;
using System.ComponentModel;
using System.Windows.Input;
using Zza.Data;
using ZzaDashboard.Services;

namespace ZzaDashboard.Customers
{
    public class CustomerEditViewModel : INotifyPropertyChanged
    {
        private Customer _customer;
        private ICustomersRepository _repository = new CustomersRepository();

        public CustomerEditViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
        }
		//a delegate trick where we assign an empty anonymous method in as a subscriber.
        //i.e. a subscriber is always in the list,so never we will have PropertyChanged being null.
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        public Customer Customer
        {
            get { return _customer; }
            set
            {
                if (value != _customer)
                {
                    _customer = value;
				//property change notifications are essential to databinding because they notify the binding when their underlying data has changed so the binding can refresh
                //and keep the data in sync with the underlying DataModel
                //There are two options for raising property change notifications that a binding will automatically monitor
                //1- make the property as a DependencyProperty, which has its own internal notification mechanism that the binding is natively aware of,
                //DependencyProperties declarations are verbose, and require our object to inherit from dependency object,
                //both of which make it a heavyweight approach to achieve the goal of change notifications in model and view model objects
                //2- use INotifyPropertyChange (INPC) : more suitable for view and view model
                    PropertyChanged(this, new PropertyChangedEventArgs("Customer"));
                }
            }
        }

        public Guid CustomerId { get; set; }
        public ICommand SaveCommand { get; set; }

        public async void LoadCustomer()
        {
            Customer = await _repository.GetCustomerAsync(CustomerId);
        }

        private async void OnSave()
        {
            Customer = await _repository.UpdateCustomerAsync(Customer);
        }

    }
}
