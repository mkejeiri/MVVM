using System;
using Unity;
using Zza.Data;
using ZzaDesktop.Customers;
using ZzaDesktop.OrderPrep;
using ZzaDesktop.Orders;
using ZzaDesktop.Services;

namespace ZzaDesktop
{
    public class MainWindowViewModel : BindableBase
    {
        /* _repo = new CustomersRepository();
         * Current parent depend on implementation that it doesn't use itself , we have to use a container
         */
        //private ICustomersRepository _repo = new CustomersRepository();
        private CustomerListViewModel _customerListViewModel;
        private OrderViewModel _orderViewModel = new OrderViewModel();
        private OrderPrepViewModel _orderPrepViewModel = new OrderPrepViewModel();
        private AddEditCustomerViewModel _addEditCustomerViewModel;
        private BindableBase _currentViewModel;

        public MainWindowViewModel()
        {
            //_addEditCustomerViewModel = new AddEditCustomerViewModel(_repo);
            //_customerListViewModel = new CustomerListViewModel(_repo);

            /*
             * The container treats IOC and DI as two phase approach to construct the object graph
             * - The IoC pattern is about delegating responsibility for construction...
             * - The Dependency Injection pattern is about providing dependencies to an object that's already been constructed...
             */
            _addEditCustomerViewModel = ContainerHelper.Container.Resolve<AddEditCustomerViewModel>();
            _customerListViewModel = ContainerHelper.Container.Resolve<CustomerListViewModel>();
            NavCommand = new RelayCommand<string>(OnNav);
            _customerListViewModel.PlaceOrderRequested += NavToOrder;
            _customerListViewModel.EditCustomerRequested += NavToEditCustomer;
            _customerListViewModel.AddCustomerRequested += NavToAddCustomer;
            _addEditCustomerViewModel.Done += NavToCustomerList;
        }

        private void NavToCustomerList()
        {
            CurrentViewModel = _customerListViewModel;
        }

        private void NavToEditCustomer(Customer customer)
        {
            _addEditCustomerViewModel.EditMode = true;
            _addEditCustomerViewModel.SetCustomer(customer);
            CurrentViewModel = _addEditCustomerViewModel;
        }

        private void NavToAddCustomer(Customer customer)
        {
            _addEditCustomerViewModel.EditMode = false;
            _addEditCustomerViewModel.SetCustomer(customer);
            CurrentViewModel = _addEditCustomerViewModel;
        }

        private void NavToOrder(Guid customerId)
        {
            _orderViewModel.CustomerId = customerId;
            CurrentViewModel = _orderViewModel;
        }

        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "orderPrep":
                    CurrentViewModel = _orderPrepViewModel;
                    break;
                case "customers":
                default:
                    CurrentViewModel = _customerListViewModel;
                    break;
            }
        }
    }
}
