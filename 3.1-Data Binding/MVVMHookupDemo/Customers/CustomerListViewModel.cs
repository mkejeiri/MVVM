using System.Collections.ObjectModel;
using System.ComponentModel;
using MVVMHookupDemo.Services;
using Zza.Data;

namespace MVVMHookupDemo.Customers
{
    public class CustomerListViewModel
    {
        private ICustomersRepository _repo = new CustomersRepository();
        private ObservableCollection<Customer> _customers;

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }

        public CustomerListViewModel()
        {
            //don't go to db in design mode
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            Customers = new ObservableCollection<Customer>(_repo.GetCustomersAsync().Result);
        }
    }
}
