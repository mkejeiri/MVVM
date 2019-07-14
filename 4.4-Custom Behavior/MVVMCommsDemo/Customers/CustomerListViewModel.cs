using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMCommsDemo.Services;
using Zza.Data;
using System.Windows.Input;

namespace MVVMCommsDemo.Customers
{
    public class CustomerListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _customers;
        private ICustomersRepository _repository = new CustomersRepository();
        
        public CustomerListViewModel()
        {
            DeleteCommand = new RelayCommand(OnDelete,CanDelete);

        }

        public async void LoadCustomers()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject())) return;

            Customers = new ObservableCollection<Customer>(await _repository.GetCustomersAsync());

        }
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
                    PropertyChanged(this, new PropertyChangedEventArgs("Customers"));
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
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
					//need to raise that event so the DeleteCommand enable/disable is refresh :  step 3
                    DeleteCommand.RaiseCanExecuteChanged();
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedCustomer"));
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


		//a delegate trick where we assign an empty anonymous method in as a subscriber.
        //i.e. a subscriber is always in the list,so never we will have PropertyChanged being null.
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
