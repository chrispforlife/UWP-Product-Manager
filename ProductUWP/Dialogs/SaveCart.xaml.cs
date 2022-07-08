using Library.TaskManagement.Services;
using ProductUWP.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ProductUWP.Dialogs
{
    public sealed partial class SaveCart : ContentDialog
    {
        public SaveCart()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {//submit cart name
            ProductService.Current.Save(CN.Text);
            ProductService.Current.SaveCarts();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //cancel
        }

    }
}
