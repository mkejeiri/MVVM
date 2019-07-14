using System.Windows.Controls;

namespace MVVMHookupDemo.Customers
{
    /// <summary>
    /// Interaction logic for CustomerListView.xaml
    /// </summary>
    public partial class CustomerListView : UserControl
    {
        public CustomerListView()
        {
            //this.DataContext = new CustomerListViewModel(); //before or after InitializeComponent doesn't matters
            InitializeComponent();
            //this.DataContext = new CustomerListViewModel();
        }
    }
}
