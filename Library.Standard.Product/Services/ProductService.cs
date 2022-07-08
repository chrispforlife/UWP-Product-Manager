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
        private List<Product> Inventory;
        private static InventoryService IService;

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
        { get { return Inventory; } }

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
            Inventory = InventoryService.Current.Inventory; //Creates inventory list
            Cart = new List<Product>(); //Creates cart list
            cartnames = new List<string>();
        }

        public void Delete(int id)
        {
            var ProductToDelete = Carts.FirstOrDefault(t => t.Id == id);

            if (ProductToDelete is ProductByQuantity)
            {
                var Quantity = ProductToDelete as ProductByQuantity;

                if (Quantity != null)
                {
                    // remove product from cart while updating both Cart, Inventory quantity and total price
                    if (!Products.Contains(Quantity))
                    {
                        Quantity.UpdateC();
                        Quantity.Calculate();
                        Inventory.Add(Quantity);
                    }
                    else if (Products.Contains(Quantity))
                    {
                        Quantity.InventoryQuantity += Quantity.CartQuantity;
                        Quantity.CartQuantity -= Quantity.CartQuantity;
                        Quantity.Calculate();
                    }
                    Carts.Remove(Quantity);

                }
            }
            else if (ProductToDelete is ProductByWeight)
            {
                var Weight = ProductToDelete as ProductByWeight;

                if (Weight != null)
                {
                    // remove product from cart while updating both Cart and Inventory quantity
                    if (!Products.Contains(Weight))
                    {
                        Weight.UpdateC();
                        Weight.Calculate();
                        Inventory.Add(Weight);
                    }
                    else if(Products.Contains(Weight))
                    {
                        Weight.IWeight += Weight.CWeight;
                        Weight.CWeight -= Weight.CWeight;
                        Weight.Calculate();
                    }
                    Carts.Remove(Weight);
                }
            }
        }
        /*
        public IEnumerable<Product> SortC()
        {
            Console.WriteLine("Would you like to sort Inventory by (N)ame or by (T)otal Price?(N/T) ");
            char c;
            while (!char.TryParse(Console.ReadLine(), out c) || (c != 'N' && c != 'T')) { Console.WriteLine("Please choose N or T "); }
            switch (c)
            {
                case 'N':
                    Console.WriteLine("Sorted by Name");
                    return Cart.OrderBy(i => i.Name);
                case 'T':
                    Console.WriteLine("Sorted by Price");
                    return Cart.OrderBy(i => i.TotalPrice);
            }
            return Cart;
        }
        */

        public IEnumerable<Product> SortCBTP()
        {
            Console.WriteLine("Sorted by TotalPrice");
            Cart = Carts.OrderBy(i => i.TotalPrice).ToList();
            return Cart;
        }

        public IEnumerable<Product> SortCBN()
        {
            Console.WriteLine("Sorted by Name");
            Cart = Carts.OrderBy(i => i.Name).ToList();
            return Cart;
        }

        public IEnumerable<Product> GetFilteredList(string query)
        { //searches for product based on name and description
            if (string.IsNullOrEmpty(query))
            {
                return Carts;
            }
            return Carts.Where(i =>
            (i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
            || (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }


        public void BOGO(Product p)
        {
            if (p.BG == true)
            {
                if (p is ProductByWeight)
                {
                    var BG = p as ProductByWeight;
                    if (BG != null)
                    {
                        BG.BOGO();
                    }
                }
                else if (p is ProductByQuantity)
                {
                    var BG = p as ProductByQuantity;

                    if (BG != null)
                    {
                        BG.BOGO();
                    }
                }
            }
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

                        if (Q.WithinStock)
                        {
                            if (!Carts.Contains(product))
                            { //if quantity is less than or equal to Inventoryquantity && not in carts,
                              // update values and adds to cart
                                Q.Calculate();
                                Carts.Add(Q);
                            }
                            else if (Carts.Contains(product))
                            { //if quantity is is less than or equal to Inventory quantity && in cart
                              //update values
                                Q.Calculate();
                            }
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

                            if (W.WithinStock)
                            {
                                if (!Carts.Contains(product))
                                { //if quantity is less than or equal to Inventoryquantity && not in carts,
                                    // update values and adds to cart
                                    W.Calculate();
                                    Carts.Add(W);
                                }
                                else if (Carts.Contains(product))
                                { //if quantity is is less than or equal to Inventory quantity && in cart
                                    //update values
                                    W.Calculate();
                                }
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
                Carts.Add(product);
            }
        }

        public double calcsubt() 
        {
            double subtotal = 0;
            foreach (Product p in Carts)
            {
                subtotal += p.TotalPrice;
            }
            return subtotal;
        }

        public double calctax() 
        {
            return (calcsubt() * 0.07);
        }

        public double calct() 
        {
            return (calcsubt() + calctax());
        }

        public void Checkout()
        { //prints checkout cart data with subtotal, tax, and total
            double subtotal = 0;
            double tax;
            double taxtotal;

            foreach (Product p in Carts)
            {
                subtotal += p.TotalPrice;
            }

            tax = (subtotal * 0.07);
            taxtotal = (subtotal * 1.07);

            Console.WriteLine($"Current subtotal without tax is: {subtotal}");
            Console.WriteLine($"Taxable amount is: {tax}");
            Console.WriteLine($"Current total with tax is : {taxtotal}\n");
            Console.WriteLine($"Current total with tax is : {taxtotal}\n");


            if (taxtotal > 0)
            {
                string cn, first, last, month, year, cvv;
                Console.WriteLine("Enter Card Information Below ");
                Console.WriteLine("Enter Card Number: ");
                cn = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter Expiration Date (MM/YY): ");
                month = Console.ReadLine() ?? string.Empty;
                year = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter Card Holder Name (Last, First): ");
                last = Console.ReadLine() ?? string.Empty;
                first = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter Card CVV number");
                cvv = Console.ReadLine() ?? string.Empty;


                string street, unit, city, state, zipcode;
                Console.WriteLine("Enter Address Details");
                Console.WriteLine("Enter Street Name: ");
                street = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter Street Unit: ");
                unit = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter Street City: ");
                city = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter Street State: ");
                state = Console.ReadLine() ?? string.Empty;
                Console.WriteLine("Enter Street Zipcode: ");
                zipcode = Console.ReadLine() ?? string.Empty;
            }
;

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