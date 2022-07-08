using System;
using Library.TaskManagement.Services;
using ProductUWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ProductUWP.Dialogs
{
    public sealed partial class ATCDialog : ContentDialog
    {
        public ATCDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductViewModel();
        }

        public ATCDialog(ProductViewModel pvm)
        {
            this.InitializeComponent();
            this.DataContext = pvm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //search for id button
            //step 1: coerce datacontext into view model
            var viewModel = DataContext as ProductViewModel;

            //step 3: interact with the service using models;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //cancel
        }
    }
}
