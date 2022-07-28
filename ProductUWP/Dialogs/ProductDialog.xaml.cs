using Library.TaskManagement.Services;
using Library.TaskManagement.Models;
using ProductUWP.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;


// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProductUWP.Dialogs
{
    public sealed partial class ProductDialog : ContentDialog
    {
        public ProductDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductViewModel();
        }

        public ProductDialog(ProductViewModel pvm)
        {
            this.InitializeComponent();
            this.DataContext = pvm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //add product button
            //step 1: coerce datacontext into view model
            var viewModel = DataContext as ProductViewModel;

            //step 2: use a conversion constructor from view model -> todo


            //step 3: interact with the service using models;
            var frame = Window.Current.Content as Frame;
            if (frame != null) 
            {
                if (frame.CurrentSourcePageType == typeof(IPage)) 
                {
                    if (viewModel.IsQuantity && !viewModel.BoundPBQ.WithinStock) 
                    { viewModel.BoundPBQ.UpdateI();}
                    else
                    if (viewModel.IsWeight && !viewModel.BoundPBW.WithinStock)
                    { viewModel.BoundPBW.UpdateI(); }

                    InventoryService.Current.AddOrUpdate(viewModel.BoundP); 
                }
                else if (frame.CurrentSourcePageType == typeof(CPage)) { ProductService.Current.AddOrUpdate(viewModel.BoundP); }
            }

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { //cancel
        }
    }
}