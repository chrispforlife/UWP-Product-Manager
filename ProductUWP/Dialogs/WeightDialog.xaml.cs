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

namespace ProductUWP.Dialogs
{
    public sealed partial class WeightDialog : ContentDialog
    {
        public WeightDialog() 
        {
            this.InitializeComponent();
            this.DataContext = new ProductByWeight();

            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CurrentSourcePageType == typeof(IPage))
                {
                    Cart_Weight.Visibility = Visibility.Collapsed;
                    CWeight.Visibility = Visibility.Collapsed;
                }
                else if (frame.CurrentSourcePageType == typeof(CPage))
                {
                    Inventory_Weight.Visibility = Visibility.Collapsed;
                    IWeight.Visibility = Visibility.Collapsed;
                    ProductTitle.Visibility = Visibility.Collapsed;
                    ProductDesc.Visibility = Visibility.Collapsed;
                    ProductPrice.Visibility = Visibility.Collapsed;
                    PT.Visibility = Visibility.Collapsed;
                    PD.Visibility = Visibility.Collapsed;
                    PP.Visibility = Visibility.Collapsed;
                }
            }
        }

        public WeightDialog(ProductViewModel selectedProduct) 
        {
            this.InitializeComponent();
            this.DataContext = selectedProduct;

            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CurrentSourcePageType == typeof(IPage))
                {
                    Cart_Weight.Visibility = Visibility.Collapsed;
                    CWeight.Visibility = Visibility.Collapsed;
                }
                else if (frame.CurrentSourcePageType == typeof(CPage))
                {
                    Inventory_Weight.Visibility = Visibility.Collapsed;
                    IWeight.Visibility = Visibility.Collapsed;
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
        {
            //step 1: coerce datacontext into view model
            var viewModel = DataContext as ProductViewModel;

            //step 2: use a conversion constructor from view model -> ProductByWeight

            //step 3: interact with the service using models;
            var frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                if (frame.CurrentSourcePageType == typeof(IPage))
                {InventoryService.Current.AddOrUpdate(viewModel.BoundPBW);}
                else if (frame.CurrentSourcePageType == typeof(CPage))
                {ProductService.Current.AddOrUpdate(viewModel.BoundPBW);}
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
