using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Zza.Data;
using ZzaDesktop.Services;

namespace ZzaDesktop.Customers
{
    public class CustomerListViewModel : BindableBase
    {
        /* _repo = new CustomersRepository();
        * Any method that touches this object is going to be really hard to unit test because it's going to try to actually go out to the database and do things.
        * we want to be able to substitute a mock object.
        * We also have another problem is that this view model is newing up its own instance of the CustomerRepository,
        * on our AddEditCustomerViewModel, it's newing up a different CustomerRepository. So the objects to get read in by those two,
        * can be completely separate objects that really represent the same rows under the covers
        * We really want a singleton model for that repository so that we're exposing shared state to the multiple view models using it
        */
        private ICustomersRepository _repo;
        private IEnumerable<Customer> _allCustomers; 

        public string SearchInput
        {
            get => _searchInput;
            set {
            SetProperty(ref _searchInput, value);
            FilterCustomer(_searchInput);
            }
          
    }

        private void FilterCustomer(string searchInput)
        {
            if (string.IsNullOrEmpty(searchInput))
            {
                Customers = new ObservableCollection<Customer>(_allCustomers);
            }
            else
            {
                searchInput = searchInput.Trim();
                Customers = new ObservableCollection<Customer>(_allCustomers.Where(c=>c.FullName.StartsWith(searchInput, StringComparison.CurrentCultureIgnoreCase)));
            }
        }

        private ObservableCollection<Customer> _customers;
        private string _searchInput;

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers , value) ;
        }

        public CustomerListViewModel(ICustomersRepository repo)
        {
            _repo = repo;
            PlaceOrderCommand = new RelayCommand<Customer>(OnPlaceOrder);
            EditCustomerCommand = new RelayCommand<Customer>(OnEditCustomer);
            AddCustomerCommand = new RelayCommand(OnAddCustomer);
            ClearSearchCommand = new RelayCommand(OnClearSearch);

        }

        private void OnClearSearch()
        {
            SearchInput = string.Empty;
            //FilterCustomer(null);
        }


        private void OnPlaceOrder(Customer customer)
        {
            PlaceOrderRequested(customer.Id);
        }

        public async void LoadCustomers()
        {
            _allCustomers = await _repo.GetCustomersAsync();
            Customers = new ObservableCollection<Customer>(_allCustomers.ToList());
        }
		/*
         * Commands on buttons have a command property to hook up to an ICommand exposed as a property on our view model that is an ICommand
         * and bind to it from the Button's Command property = > i.e. DeleteCommand property of type ICommand as the XAML gets parsed here :
         * 1 - it calls the get block on that ICommand property to get a reference to the Command object
         * 2-  it calls CanExecute on that command to determine the initial enabled or disabled state of the command, and it will enable or disable the button as a result.
         * 3-  it will subscribe to CanExecuteChanged on that ICommand, which allows it to be notified in the future if the enabled or disabled state of that command changes
         */
        //private setter because it's on set once inside the ctor
        public RelayCommand<Customer> PlaceOrderCommand { get; private set; }
        public event Action<Guid> PlaceOrderRequested = delegate { };
        public event Action<Customer> EditCustomerRequested = delegate { };
        public event Action<Customer> AddCustomerRequested = delegate { };

        private void OnAddCustomer()
        {
            AddCustomerRequested(new Customer(){Id = Guid.NewGuid()});
        }

        private void OnEditCustomer(Customer customer)
        {
            EditCustomerRequested(customer);
        }

        public RelayCommand<Customer> EditCustomerCommand { get; private set; }
        public RelayCommand AddCustomerCommand { get; private set; }

        public RelayCommand ClearSearchCommand { get; private set; }
    }
}
