using Library.Standard.Products.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    [JsonConverter(typeof(ProductJsonConverter))]
    public class ProductByQuantity: Product
    {
        public int CartQuantity { get; set; }
        public int InventoryQuantity { get; set; }

        public ProductByQuantity() 
        {
            UpdateI();
            MakeWithinStock();
        }

        public bool WithinStock //checks if product is within stock
        {
            get { return (Quantity == (CartQuantity + InventoryQuantity)); }
        }

        public void UpdateC() { InventoryQuantity = CartQuantity; CartQuantity = 0; Calculate(); } //used when product exists only in cart

        public void UpdateI() { InventoryQuantity = Quantity; CartQuantity = 0; } // used when initializing product
        public void MakeWithinStock() //updates if product is within stock
        {
            Quantity = CartQuantity + InventoryQuantity;
        }

        public void AddCQ(int i) 
        {
            if (WithinStock)
            {
                CartQuantity += i;
                InventoryQuantity -= i;
                Calculate();
            }
        }

        public void RemoveCQ() 
        {
            if (WithinStock)
            {
                InventoryQuantity += CartQuantity;
                CartQuantity -= CartQuantity;
                Calculate();
            }
        }

        public void Calculate() 
        {
            TotalPrice = Math.Round(CartQuantity * Price, 2);
            
            if (this.BG == true)
            { BOGO(); }
        }
        public void MarkBG() 
        {
            this.BG = true;
        }

        public void BOGO()
        {
            if (this.CartQuantity % 2 == 0 && this.CartQuantity >= 2)
            { //if quantity is even then price is half off as BOGO is TWO For price of one
                this.TotalPrice *= 0.50;
            }
            else if (this.CartQuantity % 2 != 0 && this.CartQuantity >= 2)
            {
                //get nearest even quantity's Total Price of the product
                var EvenTotalPrice = this.TotalPrice - this.Price;

                //implement half off and add the price of one extra product
                this.TotalPrice = (EvenTotalPrice * 0.5) + this.Price;
            }
            Console.WriteLine($"New total price is: {this.TotalPrice}");
        }

        public override string ToString()
        {
            return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} :: Description:{Description} :: Price:{Price} :: Quantity:{Quantity} :: Inventory Quantity:{InventoryQuantity} :: Cart Quantity:{CartQuantity} :: Total Price:{TotalPrice}";
        }
    }
}
