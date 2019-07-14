using System;
using System.ComponentModel.DataAnnotations;

namespace ZzaDesktop.Customers
{
    public class SimpleEditableCustomer : /*BindableBase*/ ValidatableBindableBase
    {
        private Guid _id;

        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

       
        private string _firstName;
        private string _lastName;
        private string _phone;
        private string _email;

        [Required]
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
                    SetProperty(ref _firstName, value);
                }
            }
        }

        [Required]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        [Phone]
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        [EmailAddress]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
    }
}
