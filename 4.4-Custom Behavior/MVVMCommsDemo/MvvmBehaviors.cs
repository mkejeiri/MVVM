using System.Windows;

namespace MVVMCommsDemo
{
    public class MvvmBehaviors
    {
        //alternative 1 : use the attached property and callback
        //alternative 2 : use behaviour with blend SDK
        public static string GetLoadedMethodName(DependencyObject viewWithAttachedProperty)
        {
            return (string)viewWithAttachedProperty.GetValue(LoadedMethodNameProperty);
        }

        public static void SetLoadedMethodName(DependencyObject viewWithAttachedProperty, string value)
        {
            viewWithAttachedProperty.SetValue(LoadedMethodNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for LoadedMethodNameProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadedMethodNameProperty =
            DependencyProperty.RegisterAttached("LoadedMethodName", typeof(string), typeof(MvvmBehaviors), new PropertyMetadata(null, OnLoadedMethodNameChanged));

        private static void OnLoadedMethodNameChanged(DependencyObject viewWithAttachedProperty, DependencyPropertyChangedEventArgs e)
        {
            var view = viewWithAttachedProperty as FrameworkElement;
            if (view != null)
            {
                view.Loaded += (s, e2) =>
                {
                    var viewModel = view.DataContext;
                    if (viewModel == null) return;
                    var methodInfo = viewModel.GetType().GetMethod(e.NewValue.ToString());
                    if (methodInfo != null) methodInfo.Invoke(viewModel, null);
                };
            }
        }
    }
}
