using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    public class ProductByQuantity: Product
    {
        public int CartQuantity { get; set; }
        public int InventoryQuantity { get; set; }


        public void Calculate() { TotalPrice = CartQuantity * Price; }
        public void BOGO()
        {
            if (this.BG)
            {
                if (this.CartQuantity % 2 == 0 && this.CartQuantity > 0)
                { //if quantity is even then price is half off as BOGO is TWO For price of one
                    this.TotalPrice *= 0.50;
                }
                else if (this.CartQuantity % 2 != 0 && this.CartQuantity > 0)
                {
                    //get nearest even quantity's Total Price of the product
                    var EvenTotalPrice = this.TotalPrice - this.Price;

                    //implement half off and add the price of one extra product
                    this.TotalPrice = (EvenTotalPrice * 0.5) + this.Price;
                }
                Console.WriteLine($"New total price is: {this.TotalPrice}");
            }
        }

        public override string ToString()
        {
            return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} :: Description:{Description} :: Price:{Price} :: Quantity:{Quantity} :: Inventory Quantity:{InventoryQuantity} :: Cart Quantity:{CartQuantity} :: Total Price:{TotalPrice}";
        }
    }
}
