using Library.Standard.Products.Utility;
using Library.TaskManagement.Services;
using Library.TaskManagement.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Library.TaskManagement.Services
{
    public class ProductService
    {
        private InventoryService IS = InventoryService.Current;
        private List<Product> Cart; // cart list

        private string CurrentCart { get; set; }
        private List<string> cartnames;
        private string persistPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private readonly string CNPath = "CartList";

        public string ccart
        { get { return CurrentCart; } }
         
        public List<string> CartNames
        { get { return cartnames; } }

        public List<Product> Carts
        { get { return Cart; } }

        public List<Product> Products
        { get { return IS.Inventory; } }

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
            Cart = new List<Product>(); //Creates cart list
            cartnames = new List<string>();

            var cjson = new WebRequestHandler().Get("http://localhost:5048/Cart").Result;
            if (cjson != null)
            {
                Cart = JsonConvert.DeserializeObject<List<Product>>(cjson);
            }
        }

        public void Delete(int id)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5048/Cart/Delete/{id}");
            var ProductToDelete = Cart.FirstOrDefault(t => t.Id == id);

            if (ProductToDelete is ProductByQuantity)
            {
                var Quantity = ProductToDelete as ProductByQuantity;

                if (Quantity != null)
                {
                    // remove product from cart while updating both Cart, Inventory quantity and total price
                    if (Cart.Contains(Quantity) == true)
                    {
                        Quantity.RemoveCQ();
                    }
                    else
                    if (Products.Contains(Quantity) == false)
                    {
                        Quantity.UpdateC();
                    }
                    IS.AddOrUpdate(Quantity);
                    Cart.Remove(Quantity);
                }
            }
            else if (ProductToDelete is ProductByWeight)
            {
                var Weight = ProductToDelete as ProductByWeight;

                if (Weight != null)
                {
                    // remove product from cart while updating both Cart and Inventory quantity
                    if (Cart.Contains(Weight) == true)
                    { 
                        Weight.RemoveCW();
                    }
                    else
                    if (Products.Contains(Weight) == false)
                    {
                        Weight.UpdateC();
                    }
                    IS.AddOrUpdate(Weight);
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
                        Q.AddCQ(quantity);

                        if (!Cart.Contains(product) && Q.WithinStock)
                        { //if quantity is less than or equal to Inventoryquantity && not in carts,
                          // update values and adds to cart

                            AddOrUpdate(Q);
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
                        W.AddCW(weight);

                        if (!Cart.Contains(product) && W.WithinStock)
                        { //if quantity is less than or equal to Inventoryquantity && not in carts,
                          // update values and adds to cart
                            AddOrUpdate(W);
                        }
                    }
                }
            }
        }

        public void AddOrUpdate(Product product)
        {
            /* Assignment 3
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
            */

            //Assignment 4
            var response = new WebRequestHandler().Post("http://localhost:5048/Cart/AddOrUpdate", product).Result;
            var newP = JsonConvert.DeserializeObject<Product>(response);

            var oldVersion = Cart.FirstOrDefault(i => i.Id == newP.Id);
            if (oldVersion != null)
            {
                var index = Cart.IndexOf(oldVersion);
                Cart.RemoveAt(index);
                Cart.Insert(index, newP);
            }
            else
            {
                Cart.Add(newP);
            }
            //Updates inventory version of product
            IS.AddOrUpdate(product);
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
        {
            CurrentCart = fileName;
            cartnames.Add(CurrentCart);
            var savecart = new WebRequestHandler().Get($"http://localhost:5048/Cart/Save/{fileName}").Result;
            if (savecart != null) { Cart = JsonConvert.DeserializeObject<List<Product>>(savecart); }

        }

        public void Load(string fileName = null)
        {
            CurrentCart = fileName;
            var loadcart = new WebRequestHandler().Get($"http://localhost:5048/Cart/Load/{fileName}").Result;
            Cart = JsonConvert.DeserializeObject<List<Product>>(loadcart);
        }

        public void SaveCarts()
        {
            var savecarts = new WebRequestHandler().Post($"http://localhost:5048/Cart/SaveCarts", cartnames).Result;
            cartnames = JsonConvert.DeserializeObject<List<string>>(savecarts);
        }

        public List<string> LoadCarts()
        {
            var loadcarts = new WebRequestHandler().Get($"http://localhost:5048/Cart/LoadCarts").Result;

            if (loadcarts != null) { cartnames = JsonConvert.DeserializeObject<List<string>>(loadcarts); }

            return cartnames;
        }
    }
}

/* Assignment 3 Version
namespace Library.TaskManagement.Services
{
    public class ProductService
    {
        private InventoryService IS = InventoryService.Current;
        private List<Product> Cart; // cart list

        private string CurrentCart { get; set; }
        private List<string> cartnames;
        private string persistPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private readonly string CNPath = "CartList";

        public string ccart
        { get { return CurrentCart; } }
        public List<string> CartNames
        { get { return cartnames; } }

        public List<Product> Carts
        { get { return Cart; } }

        public List<Product> Products
        { get { return IS.Inventory; } }

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
                    if (Cart.Contains(Quantity) == true)
                    {
                        Quantity.RemoveCQ();
                    }
                    else
                    if (Products.Contains(Quantity) == false)
                    {
                        Quantity.UpdateC();
                        //Products.Add(Quantity);
                    }
                    IS.AddOrUpdate(Quantity);
                    Cart.Remove(Quantity);
                }
            }
            else if (ProductToDelete is ProductByWeight)
            {
                var Weight = ProductToDelete as ProductByWeight;

                if (Weight != null)
                {
                    // remove product from cart while updating both Cart and Inventory quantity
                    if (Products.Contains(Weight) == true)
                    { //if not in inventory
                        Weight.RemoveCW();
                    }
                    else
                    if (Products.Contains(Weight) == false)
                    { //if in inventory
                        Weight.UpdateC();
                        //Products.Add(Weight);
                    }
                    IS.AddOrUpdate(Weight);
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
                        Q.AddCQ(quantity);

                        if (!Cart.Contains(product) && Q.WithinStock)
                        { //if quantity is less than or equal to Inventoryquantity && not in carts,
                          // update values and adds to cart

                            AddOrUpdate(Q);
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
                        W.AddCW(weight);

                        if (!Cart.Contains(product) && W.WithinStock)
                        { //if quantity is less than or equal to Inventoryquantity && not in carts,
                          // update values and adds to cart
                            AddOrUpdate(W);
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
*/