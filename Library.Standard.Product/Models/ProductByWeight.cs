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
    public class ProductByWeight : Product
    {
        public double Weight { get; set; }
        public double IWeight { get; set; }

        public double CWeight { get; set; }

        public ProductByWeight()
        {
            UpdateI();
            MakeWithinStock();
        }
        public bool WithinStock
        {
            get { return (Weight == (Math.Round( (CWeight + IWeight), 2))); }
        }

        public void UpdateI() { IWeight = Weight; CWeight = 0; } // used when initializing product

        public void UpdateC() { IWeight = CWeight; CWeight = 0; Calculate(); } //used when add non-existant inventory product to inventory from cart

        public void MakeWithinStock()
        {
            Weight = CWeight + IWeight;
        }

        public void AddCW(double i)
        {
            if (WithinStock)
            {
                CWeight += i;
                IWeight -= i;
                Calculate();
            }
        }

        public void RemoveCW()
        {
            if (WithinStock)
            {
                IWeight += CWeight;
                CWeight -= CWeight;
                Calculate();
            }
        }

        public void Calculate() 
        {
            TotalPrice = Math.Round(CWeight * Price, 2);
            if (this.BG == true) { BOGO(); }
        }

        public void BOGO()
        {
            double remainder = 0.0;
            this.CWeight = Math.Round(this.CWeight, 1);
            if (this.CWeight >= 2)
            {
                int whole = (int)CWeight;
                remainder = Math.Round(this.CWeight - whole, 1);

                if (whole % 2 == 0)
                {
                    //get nearest even quantity's Total Price of the product
                    var EvenTotalPrice = Math.Round(whole * this.Price, 2);

                    //implement half off and add the price of remainding weight product
                    this.TotalPrice = Math.Round((EvenTotalPrice * 0.5) + (this.Price * remainder), 2);
                }
                else if (whole % 2 != 0)
                {
                    //get nearest even quantity's Total Price of the product
                    var EvenTotalPrice = this.TotalPrice - this.Price;

                    //implement half off and add the price of one extra product
                    this.TotalPrice = (EvenTotalPrice * 0.5) + this.Price;
                }
            }
        }

        public override string ToString()
        {
            return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} ::Description:{Description} :: Price:{Price} :: Overall Weight:{Weight} :: Inventory Weight::{IWeight} :: Cart Weight:{CWeight} :: Total Price:{TotalPrice}";
        }
    }
}
