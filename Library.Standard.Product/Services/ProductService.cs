using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{
    public class ProductService
    {
        private List<Product> Cart; // cart list
        private List<Product> Invent;

        private string CurrentCart { get; set; }
        private List<string> cartnames;
        private string persistPath= $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private readonly string CNPath = "CartList";

        public string ccart
        { get { return CurrentCart; } }
        public List<string> CartNames
        { get { return cartnames; }}

        public List<Product> Carts
        { get { return Cart; } }

        public List<Product> Products
        { get { return InventoryService.Current.Inventory; } }

        private static ProductService current;
        public static ProductService Current
        {
            get
            {
                if (current == null)
                {
                    current = new ProductService();
                }
                return current;
            }
        }

        private ProductService()
        {
            this.Invent = InventoryService.Current.Inventory; //Creates inventory list
            Cart = new List<Product>(); //Creates cart list
            cartnames = new List<string>();
        }

        public void Delete(int id)
        {
            var ProductToDelete = Cart.FirstOrDefault(t => t.Id == id);

            if (ProductToDelete is ProductByQuantity)
            {
                var Quantity = ProductToDelete as ProductByQuantity;

                if (Quantity != null)
                {
                    // remove product from cart while updating both Cart, Inventory quantity and total price
                    if (Products.Contains(Quantity) == true)
                    {
                        Quantity.InventoryQuantity += Quantity.CartQuantity;
                        Quantity.CartQuantity -= Quantity.CartQuantity;
                        Quantity.Calculate();
                    }
                    else
                    if (Products.Contains(Quantity) == false)
                    {
                        Quantity.UpdateC();
                        Invent.Add(Quantity);
                    }
                    Cart.Remove(Quantity);
                }
            }
            else if (ProductToDelete is ProductByWeight)
            {
                var Weight = ProductToDelete as ProductByWeight;

                if (Weight != null)
                {
                    // remove product from cart while updating both Cart and Inventory quantity
                    if(Products.Contains(Weight) == true)
                    { //if not in inventory
                        Weight.IWeight += Weight.CWeight;
                        Weight.CWeight -= Weight.CWeight;
                        Weight.Calculate();
                    }
                    else
                    if (Products.Contains(Weight) == false)
                    { //if in inventory
                        Weight.UpdateC();
                        Products.Add(Weight);
                    }

                    Cart.Remove(Weight);
                }
            }
        }

        public IEnumerable<Product> SortCBTP()
        {
            Console.WriteLine("Sorted by TotalPrice");
            Cart = Cart.OrderBy(i => i.TotalPrice).ToList();
            return Cart;
        }

        public IEnumerable<Product> SortCBN()
        {
            Console.WriteLine("Sorted by Name");
            Cart = Cart.OrderBy(i => i.Name).ToList();
            return Cart;
        }

        public void AddtoC(Product product, int quantity)
        { //adds product from Inventory to Cart while removing completely from Inventory

            if (product is ProductByQuantity)
            {
                if (quantity <= 0)
                {
                    return;
                }
                var Q = product as ProductByQuantity;

                if (Q != null)
                {
                    if (Q.InventoryQuantity >= quantity)
                    {
                        Q.InventoryQuantity -= quantity;
                        Q.CartQuantity += quantity;
                        Q.Calculate();

                        if (!Cart.Contains(product) && Q.WithinStock)
                        { //if quantity is less than or equal to Inventoryquantity && not in carts,
                            // update values and adds to cart
                            
                            Cart.Add(Q);
                        }
                    }

                }
            }
        }

        public void AddtoC(Product product, double weight)
        { //adds product from Inventory to Cart while removing completely from Inventory

                 if (product is ProductByWeight)
                 {
                    if (weight <= 0)
                    {
                        return;
                    }

                    var W = product as ProductByWeight;

                    if (W != null)
                    {
                        W.IWeight = Math.Round(W.IWeight, 2);
                        W.CWeight = Math.Round(W.CWeight, 2);
                        if (W.IWeight >= weight)
                        {
                            W.IWeight -= weight;
                            W.CWeight += weight;
                            W.Calculate();

                            if (!Cart.Contains(product) && W.WithinStock)
                            { //if quantity is less than or equal to Inventoryquantity && not in carts,
                                // update values and adds to cart
                                Cart.Add(W);
                            }
                        }
                    }
                }
        }
        public void AddOrUpdate(Product product)
        {
            Console.WriteLine($"Current product is {product}");

            if (product.Id == 0)
            {
                if (Cart.Any())
                { //if list has elements add to product id
                    product.Id = Cart.Select(i => i.Id).Max() + 1;
                }
                else
                { //if first element make id 1
                    product.Id = 1;
                }
            }

            if (!Cart.Any(i => i.Id == product.Id))
            { //not in inventory add product
                Cart.Add(product);
            }
        }

        public double calcsubt() 
        {
            double subtotal = 0;
            foreach (Product p in Cart)
            {
                subtotal += p.TotalPrice;
            }
            return Math.Round(subtotal, 2);
        }

        public double calctax() 
        {
            return Math.Round((calcsubt() * 0.07), 2);
        }

        public double calct() 
        {
            return Math.Round(calcsubt() + calctax(), 2);
        }

        public void Save(string fileName = null)
        { //NEFARIOUS CODE
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                CurrentCart = fileName;
                cartnames.Add(CurrentCart);
                fileName = $"{persistPath}\\{fileName}.json";
            }

            var todosJson = JsonConvert.SerializeObject(Cart
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, todosJson);
        }

        public void Load(string fileName = null)
        {  //NEFARIOUS CODE
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                CurrentCart = fileName;
                fileName = $"{persistPath}\\{fileName}.json";
            }

            var ProductJson = File.ReadAllText(fileName);
            Cart = JsonConvert.DeserializeObject<List<Product>>
                (ProductJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();
        }

        public void SaveCarts() 
        {
            var fileName = $"{persistPath}\\{CNPath}.json";
            var CartJson = JsonConvert.SerializeObject(cartnames
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                File.WriteAllText(fileName, CartJson);
        }

        public void LoadCarts()
        {
            var fileName = $"{persistPath}\\{CNPath}.json";
            var CartJson = File.ReadAllText(fileName);
            cartnames = JsonConvert.DeserializeObject<List<string>>
                (CartJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<string>();
        }
    }
}