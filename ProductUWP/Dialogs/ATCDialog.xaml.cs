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

            if (viewModel.IsQuantity)
            {
                if (!viewModel.BoundPBQ.WithinStock) 
                {
                    viewModel.Quantity = viewModel.IQ + viewModel.CQ;
                }
            }
            else if (viewModel.IsWeight) 
            {
                if (!viewModel.BoundPBW.WithinStock)
                {
                    viewModel.Weight = viewModel.IW + viewModel.CW;
                }
            }
            InventoryService.Current.AddOrUpdate(viewModel.BoundP);
            ProductService.Current.AddOrUpdate(viewModel.BoundP);

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //cancel
        }
    }
}
