using Library.TaskManagement.Services;
using ProductUWP.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ProductUWP.Dialogs
{
    public sealed partial class CheckoutDialog : ContentDialog
    {
        public CheckoutDialog()
        {
            this.InitializeComponent();
            this.DataContext = this;

            SubTotal.Text = "$ "+(ProductService.Current.calcsubt());
            Tax.Text = "$ " + (ProductService.Current.calctax());
            Total.Text = "$ " + (ProductService.Current.calct());
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //log checkout
            var service = ProductService.Current;
            service.CartNames.Remove(service.ccart);
            service.SaveCarts();
            Application.Current.Exit();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //cancel
        }
    }
}
