/*
using Library.TaskManagement.Services;
using ProductUWP.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductUWP.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadCart : ContentDialog
    {
        public LoadCart()
        {
            this.InitializeComponent();
            this.DataContext = this;
            ComboBox1.ItemsSource = ProductService.Current.CartNames;
        }

        private void ComboBox1_SelectedChanged(object sender, ContentDialogButtonClickEventArgs args)
        {//submit
            ComboBox comboBox = sender as ComboBox;
            ProductService.Current.Load(comboBox.SelectedValue.ToString() );

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //submit
        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //cancel
        }
    }
} */
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
            List<String> names = new List<String>();
            ProductService.Current.LoadCarts();
            if (ProductService.Current.CartNames != null) 
            {
                foreach (var i in ProductService.Current.CartNames)
                {
                    names.Add(i);
                }

                // Finally, Specify the ComboBox items source
                ComboBox1.ItemsSource = names;
            }
           
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the ComboBox instance
            ComboBox comboBox = sender as ComboBox;
            ProductService.Current.Load(comboBox.SelectedValue.ToString());
            Frame.Navigate(typeof(CPage));
        }
    }
}