﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVMCommsDemo
{
    public static class MvvmBehaviors
    {
		//alternative 1 : use the attached property and callback
        //alternative 2 : use behaviour with blend SDK using <Interactions:CallMethodAction TargetObject="{Binding}" MethodName="LoadCustomers"/>
        public static string GetLoadedMethodName(DependencyObject obj)
        {
            return (string)obj.GetValue(LoadedMethodNameProperty);
        }

        public static void SetLoadedMethodName(DependencyObject obj, string value)
        {
            obj.SetValue(LoadedMethodNameProperty, value);
        }

        public static readonly DependencyProperty LoadedMethodNameProperty =
            DependencyProperty.RegisterAttached("LoadedMethodName",
            typeof(string), typeof(MvvmBehaviors), new PropertyMetadata(null, OnLoadedMethodNameChanged));

        //this will handle loaded event of the view
        //i.e : e.NewValue = "LoadCustomers"
        //we should set on root element like this:  local:MvvmBehaviors.LoadedMethodName="LoadCustomers">
        private static void OnLoadedMethodNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            if (element != null)
            {

                element.Loaded += (s, e2) =>
                {
                    var viewModel = element.DataContext;
                    if (viewModel == null) return;
                    var methodInfo = viewModel.GetType().GetMethod(e.NewValue.ToString());
                    if (methodInfo != null) methodInfo.Invoke(viewModel, null);
                };
            }
        }


    }
}
