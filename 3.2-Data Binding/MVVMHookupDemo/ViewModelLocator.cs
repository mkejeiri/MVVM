using System;
using System.ComponentModel;
using System.Windows;

namespace MVVMHookupDemo
{
    public static class ViewModelLocator
    {
        public static bool GetAutoWireViewModel(DependencyObject viewObject)
        {
            return (bool)viewObject.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(DependencyObject viewObject, bool value)
        {
            viewObject.SetValue(AutoWireViewModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoWireViewModelProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator),
                new PropertyMetadata(false, AutoWireViewModelChanged));

        //viewObject: is the element to which this attached property is being set, in our case it will be set in the root element of a viewObject
        //by getting  this root element of the viewObject we know which type is.
        private static void AutoWireViewModelChanged(DependencyObject viewObject, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(viewObject)) return;
            var viewType = viewObject.GetType();
            var viewTypeName = viewType.FullName;
            var viewModelTypeName = $"{viewTypeName}Model";
            var viewModelType = Type.GetType(viewModelTypeName);
            var viewModel = Activator.CreateInstance(viewModelType);
            ((FrameworkElement) viewObject).DataContext = viewModel;
        }
    }
}
