using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace MVVMCommsDemo
{
    /*ShowNotificationMessageBehaviour is acting as a bridge between MainWindowView and MainWindowViewModel
     and pushing messages between them with no code behind just a declarative notification to ContentControl using Behavior     
    HOW ?
     In Main window the ShowNotificationMessageBehaviour set on root element as follow
      <Grid>
        ...
        <ContentControl>
            <Interactions:Interaction.Behaviors>
                <local:ShowNotificationMessageBehaviour Message="{Binding NotificationMessage}"/>
            </Interactions:Interaction.Behaviors>
        </ContentControl>
        ...
      </Grid>
      where MainWindowViewModel NotificationMessage property change based on a 
      timer Elapsed event (note that MainWindowViewModel implements INPC), 
      when it fired it pushed the message value into the ShowNotificationMessageBehaviour.Message
      and cause the OnMessageChanged callback to be executed through the PropertyMetadata of 
      ShowNotificationMessageBehaviour.Message DependencyProperty as used below
     */
    public class ShowNotificationMessageBehaviour : Behavior<ContentControl> 
    {
        //below we made the Message property as a DependencyProperty
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

        //i.e e.NewValue is Message property content
        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as ShowNotificationMessageBehaviour;
            if (behavior == null) return;
            behavior.AssociatedObject.Content = e.NewValue;
            behavior.AssociatedObject.Visibility = Visibility.Visible;
        }

        //Called after the behavior is attached to an AssociatedObject.
        //message become clicked dismissible, based on MouseLeftButtonDown event
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += (s, e) => AssociatedObject.Visibility = Visibility.Collapsed;
        }     
    }
}
