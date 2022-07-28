using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ProductUWP.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductUWP.Dialogs
{
    public sealed partial class QuantityDialog : ContentDialog
    {
        public QuantityDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByQuantity();

            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CurrentSourcePageType == typeof(IPage))
                {
                    Cart_Quantity.Visibility = Visibility.Collapsed;
                    CQuantity.Visibility = Visibility.Collapsed;
                }
                else if (frame.CurrentSourcePageType == typeof(CPage))
                {
                    Inventory_Quantity.Visibility = Visibility.Collapsed;
                    IQuantity.Visibility = Visibility.Collapsed;
                    ProductTitle.Visibility = Visibility.Collapsed;
                    ProductDesc.Visibility = Visibility.Collapsed;
                    ProductPrice.Visibility = Visibility.Collapsed;
                    PT.Visibility = Visibility.Collapsed;
                    PD.Visibility = Visibility.Collapsed;
                    PP.Visibility = Visibility.Collapsed;


                }
            }
        }

        public QuantityDialog(ProductViewModel selectedProduct)
        {
            this.InitializeComponent();
            this.DataContext = selectedProduct;

            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CurrentSourcePageType == typeof(IPage))
                {
                    Cart_Quantity.Visibility = Visibility.Collapsed;
                    CQuantity.Visibility = Visibility.Collapsed;
                }
                else if (frame.CurrentSourcePageType == typeof(CPage))
                {
                    Inventory_Quantity.Visibility = Visibility.Collapsed;
                    IQuantity.Visibility = Visibility.Collapsed;
                    ProductTitle.Visibility = Visibility.Collapsed;
                    ProductDesc.Visibility = Visibility.Collapsed;
                    ProductPrice.Visibility = Visibility.Collapsed;
                    PT.Visibility = Visibility.Collapsed;
                    PD.Visibility = Visibility.Collapsed;
                    PP.Visibility = Visibility.Collapsed;
                }
            }

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //submit button
            //step 1: coerce datacontext into view model
            var viewModel = DataContext as ProductViewModel;

            //step 2: use a conversion constructor from view model -> ProductByQuantity

            //step 3: interact with the service using models;
            
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                
                if (frame.CurrentSourcePageType == typeof(IPage))
                {
                   if (viewModel.IQ > viewModel.Quantity) { viewModel.IQ = viewModel.Quantity; } 

                    InventoryService.Current.AddOrUpdate(viewModel.BoundPBQ); 
                }
                else if (frame.CurrentSourcePageType == typeof(CPage))
                {
                    if (!viewModel.BoundPBQ.WithinStock)
                    {
                        if (viewModel.CQ > viewModel.Quantity) { viewModel.CQ = viewModel.Quantity; }
                        viewModel.IQ = viewModel.Quantity - viewModel.CQ;
                        viewModel.BoundPBQ.Calculate();
                        ProductService.Current.AddOrUpdate(viewModel.BoundPBQ);
                        InventoryService.Current.AddOrUpdate(viewModel.BoundPBQ);
                    }
                }
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}