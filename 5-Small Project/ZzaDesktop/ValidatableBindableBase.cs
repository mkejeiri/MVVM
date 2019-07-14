using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ZzaDesktop
{

    //- As a general rule all the logic that supports validation and defines what rules exist for what properties, should be part of the model or the view model,
    //NOT the VIEW itself...
    //- INotifyDataErrorInfo was introduced to WPF in. NET 4.5, it supports querying the object for errors associated with properties,
    //and it fixes a couple of deficiencies with all of the other validation options (throwing exceptions, implementing IDataErrorInfo interface, validation rules). 
    //Specifically, it allows asynchronous validation and it allows properties to have more than one error associated with them.

    //ValidatableBindableBase is a base class (common to views/model view) for validation and also should have a INotifyPropertyChanged (INPC) capabilities,
    //so it will inherits from BindableBase
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        //Dictionary is an underlying datastore that support INotifyDataErrorInfo :
        // key =  property name
        // value = List of string per property name which are the errors associated with that property
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        public IEnumerable GetErrors(string propertyName)
        {
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        public bool HasErrors => _errors.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate {};

        // we need to add a trigger to say when does this thing evaluate errors? Since we're inheriting from BindableBase,
        // we can override the SetProperty method..
        protected override void SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            //to trigger INPC  
            base.SetProperty(ref member, value, propertyName);

            //And then trigger validation using data annotations
            ValidateProperty(propertyName, value);
        }

        //we could wire into this ValidateProperty any number of approaches to validation,
        // but most prominent approach in. NET applications these days is to use data annotations

        private void ValidateProperty<T>(string propertyName, T value)
        {
            //Data annotations contain the concept of ValidationContext that we can point at any given object,
            ValidationContext context = new ValidationContext(this);
            var results = new List<ValidationResult>();

            //by saying which member or property on that object is being validated,
            context.MemberName = propertyName;

            //and then call TryValidateProperty method to evaluate that object. When this method is called it will go to that property
            //on that object and see if there's any data annotation attributes for validation.
            //If any, it will execute them and get their results...
            Validator.TryValidateProperty(value, context, results);
            if (results.Any())
            {
                _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }
            //we raise the ErrorsChanged event so that the binding can go and re-query for errors
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
