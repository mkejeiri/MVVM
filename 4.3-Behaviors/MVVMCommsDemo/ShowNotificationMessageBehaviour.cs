using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace MVVMCommsDemo
{
    public class ShowNotificationMessageBehaviour : Behavior<ContentControl> 
    {
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }
        
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.RegisterAttached(nameof(Message), typeof(string), 
                typeof(ShowNotificationMessageBehaviour),
                new PropertyMetadata(null, OnMessageChanged));

        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as ShowNotificationMessageBehaviour;
            if (behavior == null) return;
            behavior.AssociatedObject.Content = e.NewValue;
            behavior.AssociatedObject.Visibility = Visibility.Visible;
        }

        //Called after the behavior is attached to an AssociatedObject.
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += (s, e) => AssociatedObject.Visibility = Visibility.Collapsed;
        }     
    }
}
