using System;
using System.ComponentModel;
using System.Data.Entity.Migrations.Model;
using Zza.Data;
using ZzaDesktop.Annotations;
using ZzaDesktop.Services;

namespace ZzaDesktop.Customers
{
    public class AddEditCustomerViewModel : BindableBase
    {
        /* _repo = new CustomersRepository();
         * Any method that touches this object is going to be really hard to unit test because it's going to try to actually go out to the database and do things.
         * we want to be able to substitute a mock object.
         * We also have another problem is that this view model is newing up its own instance of the CustomerRepository,
         * on our CustomerListViewModel, it's newing up a different CustomerRepository. So the objects to get read in by those two,
         * can be completely separate objects that really represent the same rows under the covers
         * We really want a singleton model for that repository so that we're exposing shared state to the multiple view models using it
         */
        private ICustomersRepository _repo;
        private bool _editMode;

        public bool EditMode
        {
            get => _editMode;
            set => SetProperty(ref _editMode, value);
        }

        private Customer _editingCustomer = null;
        private SimpleEditableCustomer _customer;
        public void SetCustomer(Customer customer) 
        {
            _editingCustomer = customer;

            //unsubscribe if source exists so we don't leak memory
            if (Customer !=null) Customer.ErrorsChanged -= RaiseCanExecuteChanged;
            Customer = new SimpleEditableCustomer();
            Customer.ErrorsChanged += RaiseCanExecuteChanged;
            Map(customer, Customer);
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
           SaveCommand.RaiseCanExecuteChanged();
        }

        private void Map(Customer source, SimpleEditableCustomer target)
        {
            target.Id = source.Id;
            if (EditMode)
            {
                target.FirstName = source.FirstName;
                target.LastName = source.LastName;
                target.Phone = source.Phone;
                target.Email = source.Email;
            }
        }

        public SimpleEditableCustomer Customer
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }
		/*
         * Commands on buttons have a command property to hook up to an ICommand exposed as a property on our view model that is an ICommand
         * and bind to it from the Button's Command property = > i.e. DeleteCommand property of type ICommand as the XAML gets parsed here :
         * 1 - it calls the get block on that ICommand property to get a reference to the Command object
         * 2-  it calls CanExecute on that command to determine the initial enabled or disabled state of the command, and it will enable or disable the button as a result.
         * 3-  it will subscribe to CanExecuteChanged on that ICommand, which allows it to be notified in the future if the enabled or disabled state of that command changes
         */
        //private setter because it's on set once inside the ctor
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand<Customer> AddCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public Func<Customer, bool> CanAdd { get; }

        public AddEditCustomerViewModel(ICustomersRepository repo)
        {
            _repo = repo;
            //CanSave is used for validation
            SaveCommand = new RelayCommand(OnSave, CanSave);
            AddCommand = new RelayCommand<Customer>(OnAdd, CanAdd);
            CancelCommand = new RelayCommand(OnCancel);
        }

        //signal that we are done adding/editing, so parent can handle this to drive navigation 
        public event Action Done = delegate { };

        private bool CanSave()
        {
            return !Customer.HasErrors;
        }

        private void OnCancel()
        {
            Done();
        }

        private void OnAdd(Customer customer)
        {
            Done();
        }
        

        private  async void OnSave()
        {
            UpdateCustomer(Customer, _editingCustomer);
            if (!EditMode) { 

                await _repo.AddCustomerAsync(_editingCustomer);
            }
            else await _repo.UpdateCustomerAsync(_editingCustomer);
            Done();
        }

        private void UpdateCustomer(SimpleEditableCustomer source, Customer target)
        {
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Email = source.Email;
            target.Phone = source.Phone;
        }
    }
}
