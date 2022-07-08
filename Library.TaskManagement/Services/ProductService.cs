using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{
    public class ProductService
    {
        private List<Product> Cart; // cart list
        private List<Product> Inventory;
        private string _persistPath = "cart.json";

        public List<Product> Carts
        { get { return Cart; } }

        public List<Product> Products
        { get { return Inventory; } }

        private static ProductService? current;

        //public static
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
                    Quantity.InventoryQuantity += Quantity.CartQuantity;
                    Quantity.CartQuantity -= Quantity.CartQuantity;
                    Quantity.Calculate();
                    Carts.Remove(Quantity);
                    Console.WriteLine($"Product {Quantity} \nWas Deleted From Cart And Was Added To Inventory");
                    Products.Add(Quantity);
                }
            }
            else if (ProductToDelete is ProductByWeight)
            {
                var Weight = ProductToDelete as ProductByWeight;

                if (Weight != null)
                {
                    // remove product from cart while updating both Cart and Inventory quantity
                    Weight.Quantity += Weight.Weight;
                    Weight.Weight -= Weight.Weight;
                    Weight.Calculate();
                    Carts.Remove(Weight);
                    Console.WriteLine("Product Was Deleted From Cart And Was Added To Inventory");
                    Products.Add(Weight);
                }
            }
        }

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
                    return Cart.OrderBy(i => i.Price);
            }
            return Cart;
        }


        public IEnumerable<Product> GetFilteredList(string? query)
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
        public void AddtoC(Product product)
        { //adds product from Inventory to Cart while removing completely from Inventory

            //quantity of product to be added
            var quantity = 0;
            var weight = 0.0;

            if (product is ProductByQuantity)
            {
                Console.WriteLine("What quantity of this product would you like to add to cart?");
                quantity = int.Parse(Console.ReadLine() ?? "0");
                if (quantity <= 0)
                {
                    return;
                }
                var Q = product as ProductByQuantity;

                if (Q != null)
                {
                    if (Q.InventoryQuantity >= quantity)
                    { //if quantity is identical, remove from Inventory and add to Carts

                        Q.InventoryQuantity -= quantity;
                        Q.CartQuantity += quantity;
                        Q.Calculate();
                        BOGO(Q);
                        Products.Remove(Q);
                        Carts.Add(Q);
                    }
                    else
                    {
                        Console.WriteLine("Quantity is not available in inventory");
                    }
                }
            }
            else if (product is ProductByWeight)
            {
                Console.WriteLine("What quantity of this product would you like to add to cart?");
                weight = double.Parse(Console.ReadLine() ?? "0");
                if (weight <= 0)
                {
                    return;
                }

                var W = product as ProductByWeight;

                if (W != null)
                {
                    if (W.Quantity >= weight)
                    { //if quantity is identical, remove from Inventory and add to Carts
                        W.Quantity -= weight;
                        W.Weight += weight;
                        W.Calculate();
                        BOGO(W);
                        Products.Remove(W);
                        Carts.Add(W);

                    }
                    else
                    {
                        Console.WriteLine("Weight is not available in inventory");
                    }
                }
            }
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

        public void Save()
        { //NEFARIOUS CD
            var ProductJson = JsonConvert.SerializeObject(Carts
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(_persistPath, ProductJson);
        }

        public void Load()
        {  //NEFARIOUS CODE
            var ProductJson = File.ReadAllText(_persistPath);
            Cart = JsonConvert.DeserializeObject<List<Product>>
                (ProductJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();
        }

    }
}

/*
using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{
    public class ProductService
    {
        private List<Product> Inventory; //inventory list
        private List<Product> Cart; //cart list


        private ListNavigator<Product> IlistNavigator;
        private ListNavigator<Product> ClistNavigator;



        public List<Product> Products
        { get { return Inventory; } }

        public List<Product> Purchase
        { get { return Cart; } }

        public int NextId
        {
            get
            {
                if (!Products.Any())
                    return 1;

                return Products.Select(t => t.Id).Max() + 1;
            }
        }

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
            Inventory = new List<Product>(); //Creates inventory list
            Cart = new List<Product>(); //Creates cart list
            IlistNavigator = new ListNavigator<Product>(Inventory);
            ClistNavigator = new ListNavigator<Product>(Cart);

        }

        public void Create(Product product)
        {
            if (Products.Any(n => n.Name == product.Name))
            //checks if product name exists
            {
                Console.WriteLine("Product exists!");
                Console.WriteLine("Would you like to update product quantity (Y/N)?");
                char c;
                while (!char.TryParse(Console.ReadLine(), out c) || (c != 'Y' && c != 'N')) { Console.WriteLine("Please choose either Y/N"); }

                if (c == 'Y')
                {
                    if (product is ProductByWeight)
                    { //find index of current product and update both weight * price
                        int index = Products.FindIndex(n => n.Name == product.Name);
                        var sameWeight = Products[index] as ProductByWeight;

                        if (sameWeight != null && sameWeight.GetType()==Products[index].GetType())
                        {
                            double Weight;
                            Console.WriteLine("What is the number of units being purchased for the Product (Weight)(Enter negative value to show loss) ?");
                            while (!double.TryParse(Console.ReadLine(), out Weight)) { Console.WriteLine("Please Enter An Integer"); }

                            //find index of current product and update both weight * quantity
                            sameWeight.Weight += Weight;
                            sameWeight.Calculate();
                            //sameWeight.TotalPrice = sameWeight.Price * sameWeight.Weight;
                        }
                        else { Console.WriteLine("Current Product doesn't utilize Weight."); }
                    }
                    else if (product is ProductByQuantity)
                    {//find index of current product and update both price * quantity
                        int index = Products.FindIndex(n => n.Name == product.Name);
                        var sameQuantity = product as ProductByQuantity;

                        if (sameQuantity != null && sameQuantity.GetType() == Products[index].GetType())
                        {
                            int Quantity;
                            Console.WriteLine("What is the number of units being purchased for the Product (Quantity)(Enter negative value to show loss) ?");
                            while (!int.TryParse(Console.ReadLine(), out Quantity)) { Console.WriteLine("Please Enter An Integer"); }

                            //update overall quantity, inventory quantity, and total price
                            sameQuantity.InventoryQuantity += Quantity;
                            sameQuantity.Quantity += Quantity;
                            sameQuantity.Calculate();
                            //sameQuantity.TotalPrice = sameQuantity.Price * sameQuantity.CartQuantity;
                        }
                        else { Console.WriteLine("Current Product doesn't utilize Quantity."); }
                    }
                }
            }
            else
            {
                Console.WriteLine("Product doesn't exist!");
                product.Id = NextId;
                Products.Add(product);
            }
        }

        public void ReadI()
        {
            Console.WriteLine("Current Products in Inventory");
            IlistNavigator.IteratePages();
        }

        public void ReadC()
        {
            Console.WriteLine("Current Products in Cart");
            ClistNavigator.IteratePages();
        }


        public void Update(Product? product) //update product in inventory
        {

        }

        public void AddtoC(Product product, dynamic quantity)
        { //adds product from Inventory to Cart while removing completely from Inventory

            if (product is ProductByQuantity)
            {
                var Q = product as ProductByQuantity;

                if (Q != null)
                {
                    if ((Q.InventoryQuantity >= quantity))
                    { //if quantity is identical, remove from Inventory and add to Carts
                        Products.Remove(Q);
                        Q.InventoryQuantity -= quantity;
                        Q.CartQuantity += quantity;
                        Q.Calculate();
                        //Q.TotalPrice = Q.CartQuantity * Q.Price;
                        Purchase.Add(Q);
                    }
                    else 
                    {
                        Console.WriteLine("Quantity is not available in inventory");
                    }
                }
            }
            else if (product is ProductByWeight)
            {
                var W = product as ProductByWeight;

                if (W != null)
                {
                    if ((W.Weight - quantity >= 0))
                    { //if quantity is identical, remove from Inventory and add to Carts
                        if (Products.Remove(W))
                        {
                            Purchase.Add(W);
                        }
                    }
                    else 
                    {
                        Console.WriteLine("Weight is not available in inventory");
                    }
                }
            }
        }

        public void Delete()
        { //delete a product based on id
            Console.WriteLine("Delete from (I)nventory or (C)arts?(I/C)");
            char c;
            while (!char.TryParse(Console.ReadLine(), out c) || (c != 'I' && c != 'C')) { Console.WriteLine("Please choose either I/C"); }

            Console.WriteLine("Which Product would you like to delete?(ID)");
            var ID = int.Parse(Console.ReadLine() ?? "0");

            if (c == 'I')  //removes Product if in inventory
            {
                var ProductToDelete = Products.FirstOrDefault(t => t.Id == ID);
                if (ProductToDelete == null)
                { //if product isn't found then exit
                    Console.WriteLine("Product Does Not Exist");
                    return;
                }

                Console.WriteLine("Product Was Deleted From Inventory");
                Products.Remove(ProductToDelete);
            }
            else
                if (c == 'C')  //removes Product from cart and places it into Inventory
            {

                var ProductToDelete = Purchase.FirstOrDefault(t => t.Id == ID);
                if (ProductToDelete == null)
                { //if product isn't found then exit
                    Console.WriteLine("Product Does Not Exist");
                    return;
                }

                if (ProductToDelete is ProductByQuantity)
                {
                    var Quantity = ProductToDelete as ProductByQuantity;

                    if (Quantity != null)
                    {
                        // remove product from cart while updating both Cart, Inventory quantity and total price
                        Purchase.Remove(Quantity);
                        Quantity.InventoryQuantity += Quantity.CartQuantity;
                        Quantity.CartQuantity -= Quantity.CartQuantity;
                        Quantity.Calculate();
                        //Quantity.TotalPrice = Quantity.CartQuantity * Quantity.Price;
                        Console.WriteLine("Product Was Deleted From Cart And Was Added To Inventory");
                        Products.Add(Quantity);
                    }
                }
                else if (ProductToDelete is ProductByWeight)
                {
                    var Weight = ProductToDelete as ProductByWeight;

                    if (Weight != null)
                    {
                        // remove product from cart while updating both Cart and Inventory quantity
                        Purchase.Remove(Weight);
                        Console.WriteLine("Product Was Deleted From Cart And Was Added To Inventory");
                        Products.Add(Weight);
                    }
                }

            }
        }

        private string _query;
        private bool _sort;
        public IEnumerable<Product> SearchI(string query) 
        {
            _query = query;
            return IProcessedList;
        }

        public IEnumerable<Product> SearchC(string query)
        {
            _query = query;
            return CProcessedList;
        }
        public IEnumerable<Product> IProcessedList 
        {
            get
            {
                if (string.IsNullOrEmpty(_query))
                {
                    return Products;
                }
                return Products
                    .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                        || i.Name.Contains(_query))) //search
                    .OrderBy(i => i.Name);
            }
        }

        public IEnumerable<Product> CProcessedList
        {
            get
            {
                if (string.IsNullOrEmpty(_query))
                {
                    return Purchase;
                }
                return Purchase
                    .Where(i => string.IsNullOrEmpty(_query) || (i.Description.Contains(_query)
                        || i.Name.Contains(_query))) //search
                    .OrderBy(i => i.Name);
            }
        }

        public void BOGO(Product p)
        {
            p.BG = true;

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

        public void Checkout()
        { //prints checkout cart data with subtotal, tax, and total
            double subtotal = 0;
            double tax;
            double taxtotal;

            foreach(Product p in Purchase) 
            {
                subtotal += p.TotalPrice;
            }

            tax = (subtotal * 0.07);
            taxtotal = (subtotal * 1.07);

            Console.WriteLine($"Current subtotal without tax is: {subtotal}");
            Console.WriteLine($"Taxable amount is: {tax}");
            Console.WriteLine($"Current total with tax is : {taxtotal}");


            if (taxtotal < 0)
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

        public void Save(string fileName) 
        {
            var ProductJson = JsonConvert.SerializeObject(Purchase);
            File.WriteAllText(fileName, ProductJson);
        }

        public void Load(string fileName)
        {
            var ProductJson = File.ReadAllText(fileName);
            Cart = JsonConvert.DeserializeObject<List<Product>>(ProductJson) ?? new List<Product>();
        }

    }
}
*/