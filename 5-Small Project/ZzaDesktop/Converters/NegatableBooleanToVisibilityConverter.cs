using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ZzaDesktop.Converters
{
    //Alternative 1 : Pb of separation of concern
    //Rule 1 : it's the view's job to dictate what the user sees on the screen
    //Rule 2: the view model just provides the interaction logic and the data manipulation to support that
    //==> WE CANNOT toggle text property of add/save from the view, later we could decide to have icons for instance

    //Alternative 2 :
    //We want to bind the Visibility property of the button add/save for add/edit customer,
    //but we want it to be driven by a Boolean flag,now there is a built-inBooleanToVisibilityConverter we could use,
    //unfortunately it doesn't let you negate what your flag means, and it also is hardcoded to always use Collapsed as the false state.

    //Alternative 3 : seems more viable option
    //we use calledNegatableBooleanToVisibilityConverter

    public class NegatableBooleanToVisibilityConverter : IValueConverter
    {
        public NegatableBooleanToVisibilityConverter()
        {
            //default to Collapsed
            FalseVisibility = Visibility.Collapsed;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isValueParsed = bool.TryParse(value.ToString(), out var isEditable);
            if (!isValueParsed) return value;

            if (isEditable && !Negate ) return Visibility.Visible;
            if (isEditable && Negate) return FalseVisibility;
            if (!isEditable && Negate) return Visibility.Visible;
            if (!isEditable && !Negate) return FalseVisibility;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        //negate the visibility
        public bool Negate { get; set; }
        //drive the visibility
        public Visibility FalseVisibility { get; set; }
    }
}
