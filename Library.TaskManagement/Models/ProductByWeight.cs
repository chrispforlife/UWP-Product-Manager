using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    public class ProductByWeight : Product
    {
        public new double Quantity { get; set; }
        public double Weight { get; set; }

        public void Calculate() {TotalPrice = Weight * Price;}

        public void BOGO()
        {
            int whole = (int)Math.Round(this.Weight, 0);
            double remainder = this.Weight -= whole;
            if (whole % 2 == 0 && whole > 0)
            if (this.Weight % 2 == 0 && this.Weight > 0)
            { //if quantity is even then price is half off as BOGO is TWO For price of one
                this.TotalPrice *= 0.50;
            }
            else if (whole % 2 != 0 && whole > 0)
            {
                //get nearest even quantity's Total Price of the product
                var EvenTotalPrice = (this.Price * whole) - this.Price;

                //implement half off and add the price of one extra product
                this.TotalPrice = (EvenTotalPrice* 0.5) +(this.Price) + (this.Price*remainder);
            }
        }

        public override string ToString()
        {
            return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} ::Description:{Description} :: Price:{Price} :: Quantity::{Quantity} :: Weight:{Weight} :: Total Price:{TotalPrice}";
        }
    }
}
