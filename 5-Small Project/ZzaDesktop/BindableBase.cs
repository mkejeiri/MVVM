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
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
       
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
