using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using System;
using Library.TaskManagement.Services;


namespace ProductUWP
{
    public sealed partial class LoadCart : Page
    {
        public LoadCart()
        {
            this.InitializeComponent();
            List<String> names = new List<string>();

            names = ProductService.Current.LoadCarts();
            names.Add("Default");

            if (names != null)
            {
                ComboBox1.ItemsSource = names;
            }

            /*
            if (ProductService.Current.CartNames != null) 
            {
                foreach (var i in ProductService.Current.CartNames)
                {
                    names.Add(i);
                }

                // Finally, Specify the ComboBox items source
                ComboBox1.ItemsSource = names;
            }
            */
           
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the ComboBox instance
            ComboBox comboBox = sender as ComboBox;

            if (comboBox.SelectedValue.ToString() != "Default") 
            {
                ProductService.Current.Load(comboBox.SelectedValue.ToString());
                Frame.Navigate(typeof(CPage));
            }
            else 
            {
                Frame.Navigate(typeof(CPage));
            }
            
        }
    }
}