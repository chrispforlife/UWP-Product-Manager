using Library.TaskManagement.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace ProductUWP.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return BoundP?.Name ?? string.Empty;
            }

            set
            {
                if (BoundP == null)
                {
                    return;
                }

                BoundP.Name = value;
            }
        }

        public string Description
        {
            get
            {
                return BoundP?.Description ?? string.Empty;
            }

            set
            {
                if (BoundP == null)
                {
                    return;
                }

                BoundP.Description = value;
            }
        }

        public double Price
        {
            get
            {
                return BoundP?.Price ?? 0.0;
            }

            set
            {
                if (BoundP == null)
                {
                    return;
                }

                BoundP.Price = value;
            }
        }

        public int Quantity
        {
            get
            {
                return BoundP?.Quantity ?? 0;
            }

            set
            {
                if (BoundP == null)
                {
                    return;
                }

                BoundP.Quantity = value;
            }
        }

        public int IQ
        {
            get
            {
                return boundPBQ?.InventoryQuantity ?? 0;
            }

            set
            {
                if (boundPBQ == null)
                {
                    return;
                }

                boundPBQ.InventoryQuantity = value;
            }
        }

        public int CQ
        {
            get
            {
                return boundPBQ?.CartQuantity ?? 0;
            }

            set
            {
                if (boundPBQ == null)
                {
                    return;
                }

                boundPBQ.CartQuantity = value;
            }
        }

        public double IW
        {
            get
            {
                return boundPBW?.IWeight ?? 0.0;
            }

            set
            {
                if (boundPBW == null)
                {
                    return;
                }

                boundPBW.IWeight = value;
            }
        }

        public double CW
        {
            get
            {
                return boundPBW?.CWeight ?? 0.0;
            }

            set
            {
                if (boundPBW == null)
                {
                    return;
                }

                boundPBW.CWeight = value;
            }
        }

        public double Weight
        {
            get
            {
                return boundPBW?.Weight ?? 0.0;
            }

            set
            {
                if (boundPBW == null)
                {
                    return;
                }

                boundPBW.Weight = value;
            }
        }

        public double TotalPrice
        {
            get
            {
                return BoundP?.TotalPrice ?? 0.0;
            }

            set
            {
                if (BoundP == null)
                {
                    return;
                }

                BoundP.TotalPrice = value;
            }
        }
        public bool BG
        {
            get
            {
                return BoundP?.BG ?? false;
            }

            set
            {
                if (BoundP == null)
                {
                    return;
                }

                BoundP.BG = value;
            }
        }

        public int Id
        {
            get { return BoundP?.Id ?? 0; }
        }

        public string CN 
        {
            get; set;
        }

        public void ATC() 
        {
            if (IsWeight) 
            {
                
            }
            else if (IsQuantity) 
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            if (IsWeight) 
            { return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} ::Description:{Description} :: Price:{Price} :: Overall Weight:{Weight} :: Inventory Weight::{IW} :: Cart Weight:{CW} :: Total Price:{TotalPrice}"; }
            if (IsQuantity)
            {
                return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} :: Description:{Description} :: Price:{Price} :: Quantity:{Quantity} :: Inventory Quantity:{IQ} :: Cart Quantity:{CQ} :: Total Price:{TotalPrice}";
            }
            else 
            {
                return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} :: Description:{Description} :: Price:{Price} :: Quantity:{Quantity}"; 
            }
        }



        public Product BoundP
        {
            get
            {
                if (BoundPBQ != null)
                {
                    return BoundPBQ;
                }
                return BoundPBW;
            }
        }

        public Visibility IsQuantityCardVisible
        {
            get
            {
                return boundPBQ == null && boundPBW != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility IsWeightCardVisible
        {
            get
            {
                return boundPBW == null && boundPBQ != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public bool IsQuantity
        {
            get
            {
                return boundPBQ != null;
            }

            set
            {
                if (value)
                {
                    boundPBQ = new ProductByQuantity();
                    boundPBW = null;
                    NotifyPropertyChanged("IsWeightCardVisible");
                    NotifyPropertyChanged("IsQuantityCardVisible");
                }

            }
        }

        public bool IsWeight
        {
            get
            {
                return boundPBW != null;
            }

            set
            {
                if (value)
                {
                    boundPBW = new ProductByWeight();
                    boundPBQ = null;
                    NotifyPropertyChanged("IsWeightCardVisible");
                    NotifyPropertyChanged("IsQuantityCardVisible");
                }

            }
        }

        private ProductByQuantity boundPBQ;
        public ProductByQuantity BoundPBQ
        {
            get 
            {
                return boundPBQ;
            }
        }

        private ProductByWeight boundPBW;
        public ProductByWeight BoundPBW
        {
            get 
            {
                return boundPBW; 
            }
        }


        public ProductViewModel()
        {
            boundPBQ = new ProductByQuantity();
            boundPBW = null;
        }


        public ProductViewModel(Product p) 
        {
            if(p == null) 
            {
                return;
            }

            if (p is ProductByQuantity) 
            {
                boundPBQ = p as ProductByQuantity;
            }
            else if (p is ProductByWeight)
            {
                boundPBW = p as ProductByWeight;
            }
        }

    }
}