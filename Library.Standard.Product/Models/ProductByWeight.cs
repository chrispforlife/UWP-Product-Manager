using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    public class ProductByWeight : Product
    {
        public double Weight { get; set; }
        public double IWeight { get; set; }

        public double CWeight { get; set; }

        public bool WithinStock
        {
            get { return (Weight == (CWeight + IWeight)); }
        }

        public void UpdateC() { Weight = CWeight; IWeight = Weight; } //used when product exists only in cart
        public void UpdateI() { IWeight = Weight; } // used when initializing product
        public void Calculate() 
        {
            if (this.BG == false) 
            { TotalPrice = CWeight * Price; }
            else if (this.BG == true) 
            {
                BOGO();
            }
        }

        public void BOGO()
        {
            if (this.BG)
            {
                double remainder = 0.0;
                if (this.CWeight > 2)
                {
                    int whole = (int)Math.Round(this.CWeight, 0);
                    remainder = this.CWeight - whole;

                    if (whole % 2 == 0 && whole >= 0)
                    {
                        //get nearest even quantity's Total Price of the product
                        var EvenTotalPrice = Math.Round(whole * this.Price, 2);

                        //implement half off and add the price of remainding weight product
                        this.TotalPrice = Math.Round((EvenTotalPrice * 0.5) + (this.Price * remainder), 2);
                    }
                }
                
            }
        }

        public override string ToString()
        {
            return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} ::Description:{Description} :: Price:{Price} :: Overall Weight:{Weight} :: Inventory Weight::{IWeight} :: Cart Weight:{CWeight} :: Total Price:{TotalPrice}";
        }
    }
}
