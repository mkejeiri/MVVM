using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Zza.Data
{
    public class Customer :INotifyPropertyChanged
    {
        public Customer()
        {
            Orders = new List<Order>();
        }
        [Key]
        public Guid Id { get; set; }
        public Guid? StoreId { get; set; }
        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
				//property change notifications are essential to databinding because they notify the binding when their underlying data has changed so the binding can refresh
                //and keep the data in sync with the underlying DataModel
                //There are two options for raising property change notifications that a binding will automatically monitor
                //1- make the property as a DependencyProperty, which has its own internal notification mechanism that the binding is natively aware of,
                //DependencyProperties declarations are verbose, and require our object to inherit from dependency object,
                //both of which make it a heavyweight approach to achieve the goal of change notifications in model and view model objects
                //2- use INotifyPropertyChange (INPC) : more suitable for view and view model
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                }
            }
        }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public List<Order> Orders { get; set; }
		
		//a delegate trick where we assign an empty anonymous method in as a subscriber.
        //i.e. a subscriber is always in the list,so never we will have PropertyChanged being null.
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
