using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ProductUWP.Dialogs;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace ProductUWP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public string Query { get; set; }
        public ProductViewModel SelectedProduct { get; set; }
        private InventoryService _IService;
        private ProductService _CService;

        public ObservableCollection<ProductViewModel> Inventory
        {
            get
            {
                if (_IService == null)
                {
                    return new ObservableCollection<ProductViewModel>();
                }

                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ProductViewModel>(_IService.Inventory.Select(p => new ProductViewModel(p)));
                }
                else
                {
                    return new ObservableCollection<ProductViewModel>(
                        _IService.Inventory.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .Select(p => new ProductViewModel(p)));
                }
            }
        }

        public ObservableCollection<ProductViewModel> Carts
        {
            get
            {
                if (_CService == null)
                {
                    return new ObservableCollection<ProductViewModel>();
                }

                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ProductViewModel>(_CService.Carts.Select(p => new ProductViewModel(p)));
                }
                else
                {
                    return new ObservableCollection<ProductViewModel>(
                        _CService.Carts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .Select(p => new ProductViewModel(p)));
                }
            }
        }

        public MainViewModel()
        {
            _IService = InventoryService.Current;
            _CService = ProductService.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Add(ProductType iType)
        {
            ContentDialog diag = null;

            if (iType == ProductType.Product)
            {
                diag = new ProductDialog();
            }
            else
            {
                throw new NotImplementedException();
            }

            await diag.ShowAsync();
            NotifyPropertyChanged("Inventory");
        }

        public void IRemove()
        {
            var id = SelectedProduct?.Id ?? -1;
            if (id >= 1)
            {
                _IService.Delete(SelectedProduct.Id);
            }
            NotifyPropertyChanged("Inventory");
        }

        public void IBOGO()
        {
            if (SelectedProduct != null)
            {
                _IService.MarkBOGO(SelectedProduct.BoundP);
                NotifyPropertyChanged("Inventory");
            }
        }

        public async void IUpdate()
        {
            if (SelectedProduct != null)
            {
                ContentDialog diag = null;
                if (SelectedProduct.IsWeight)
                { diag = new WeightDialog(SelectedProduct); }
                else if (SelectedProduct.IsQuantity)
                { diag = new QuantityDialog(SelectedProduct); }

                await diag.ShowAsync();
                NotifyPropertyChanged("Inventory");
            }
        }

        public void ISave()
        {
            _IService.Save();
        }

        public void ILoad()
        {
            _IService.Load();
            NotifyPropertyChanged("Inventory");
        }

        public void ISortbyName()
        {
            _IService.SortIBN();
            NotifyPropertyChanged("Inventory");
        }

        public void SortbyPrice()
        {
            _IService.SortIBP();
            NotifyPropertyChanged("Inventory");
        }

        public void ATC()
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.IsWeight)
                { 
                    
                    _CService.AddtoC(SelectedProduct.BoundPBW, 0.1); 
                }
                else if (SelectedProduct.IsQuantity)
                { _CService.AddtoC(SelectedProduct.BoundPBQ, 1); }
                Refresh();
            }
        }

        public void CRemove()
        {
            var current = _CService.Carts.FirstOrDefault(i => i.Id == SelectedProduct.Id);
            if (current != null)
            {
                var id = SelectedProduct?.Id ?? -1;
                if (id >= 1)
                {
                    _CService.Delete(SelectedProduct.Id);
                }
                Refresh();
            }
        }
        public async void CUpdate()
        {
            var current = _CService.Carts.FirstOrDefault(i => i.Id == SelectedProduct.Id);
            if (SelectedProduct != null && current != null)
            {
                ContentDialog diag = null;
                if (SelectedProduct.IsWeight)
                { diag = new WeightDialog(SelectedProduct); }
                else if (SelectedProduct.IsQuantity)
                { diag = new QuantityDialog(SelectedProduct); }

                await diag.ShowAsync();
                Refresh();
            }
        }
        public void CSortbyName()
        {
            _CService.SortCBN();
            NotifyPropertyChanged("Carts");
        }

        public void SortTP_Click()
        {
            _CService.SortCBTP();
            NotifyPropertyChanged("Carts");
        }

        public async void CSave()
        {
            ContentDialog diag = new SaveCart();
            await diag.ShowAsync();
        }

        public async void CLoad()
        {

            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(LoadCart));
            NotifyPropertyChanged("Carts");
        }

        public async void Checkout()
        {
            ContentDialog diag = new CheckoutDialog();
            await diag.ShowAsync();
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Inventory");
            NotifyPropertyChanged("Carts");
        }

    }
}

    public enum ProductType
    {
        Weight, Quantity, Product
    }
