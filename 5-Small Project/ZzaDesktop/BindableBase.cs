using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZzaDesktop
{
    //BindableBase is to encapsulate the implementation of INPC for the 3 ViewModels.
    //1- encapsulate the INotifyPropertyChanged implementation
    //2- provide helper methods to the derive class so that they can easily trigger the appropriate notifications
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void SetProperty<T>(ref T member, T value,
            //That attribute makes it so it can automatically pick off the name of the property that called in
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member,value)) return;
            member = value;
				//property change notifications are essential to databinding because they notify the binding when their underlying data has changed so the binding can refresh
                //and keep the data in sync with the underlying DataModel
                //There are two options for raising property change notifications that a binding will automatically monitor
                //1- make the property as a DependencyProperty, which has its own internal notification mechanism that the binding is natively aware of,
                //DependencyProperties declarations are verbose, and require our object to inherit from dependency object,
                //both of which make it a heavyweight approach to achieve the goal of change notifications in model and view model objects
                //2- use INotifyPropertyChange (INPC) : more suitable for view and view model
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
       
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
