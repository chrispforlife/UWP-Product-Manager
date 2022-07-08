using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;

namespace TaskManagement2022.Helpers
{
    internal class Helpers
    {
        internal static void ListItems(IEnumerable<object> list)
        {
            var List = list.ToList();
            if (List != null && List.Any())
            { // if list is not empty or null
                ListNavigator<object> listNavigator = new ListNavigator<object>(List);
                listNavigator.IteratePages();
            }
            else
                Console.WriteLine("Empty List");
        }
        /*
        internal static Product FillInventoryItem(Product? invItem) 
        {
            Console.WriteLine("What is the name of the product?");
            var name = Console.ReadLine();

            Console.WriteLine("What is the description of the product?");
            var desc = Console.ReadLine();

            while (true)
            {
                Console.WriteLine("What is the price of the product?");
                if (double.TryParse(Console.ReadLine(), out double price))
                {
                    Console.WriteLine("How many are available?");
                    var quant = 1;
                    if(!int.TryParse(Console.ReadLine(), out quant)) 
                    {
                        Console.WriteLine("Does not compute -- defaulting quantity to 1");
                    }
                    if (invItem == null) 
                    {
                        return new Product
                        {
                            Name = name ?? string.Empty,
                            Description = desc ?? string.Empty,
                            Price = price,
                            Quantity = quant

                        };
                    }

                    invItem.Name = name ?? string.Empty;
                    invItem.Description = desc ?? string.Empty;
                    invItem.Price = price;
                    invItem.Quantity = quant;
                    return invItem;
                }
            }
        }
        */
        internal static Product FillInventoryItem(Product? newProduct)
        {
            Console.WriteLine("What is the name for the Product?");
            var name = Console.ReadLine() ?? string.Empty;
            newProduct.Name = name;

            Console.WriteLine("What is the description for the Product?");
            var desc = Console.ReadLine() ?? string.Empty;
            newProduct.Description = desc;

            double price;
            Console.WriteLine("What is the unit price for the Product?");
            while (!double.TryParse(Console.ReadLine(), out price)) { Console.WriteLine("Please Enter A Double"); }
            newProduct.Price = price;

            if (newProduct is ProductByQuantity)
            {
                var newQuantity = newProduct as ProductByQuantity;

                if (newQuantity != null)
                { //only prompt for inventory quantity
                    int IQuantity;
                    Console.WriteLine("What is the number of units in the Inventory for the Product? (Quantity)");
                    while (!int.TryParse(Console.ReadLine(), out IQuantity)) { Console.WriteLine("Please Enter An Integer"); }
                    newQuantity.InventoryQuantity = IQuantity;

                    //initalize quantity as inventory 
                    newQuantity.Quantity = IQuantity;

                    //set total price to 0 as product isn't in cart
                    //newQuantity.TotalPrice = 0;
                }
            }
            else if (newProduct is ProductByWeight)
            {
                var newWeight = newProduct as ProductByWeight;

                if (newWeight != null)
                {
                    double weight;
                    Console.WriteLine("What is the number of units being purchased for the Product? (Weight)");
                    while (!double.TryParse(Console.ReadLine(), out weight)) { Console.WriteLine("Please Enter An Double"); }
                    newWeight.Weight = weight;

                    //multiply weight and price to receive total price
                    newWeight.Calculate();
                    //newWeight.TotalPrice = newWeight.Price * newWeight.Weight;
                }
            }
            
            if (newProduct == null)
            {
                return new Product
                {
                    Name = name ?? string.Empty,
                    Description = desc ?? string.Empty,
                    Price = price,
                    BG = false
                };
            }

            return newProduct;
        }

        public static void FillProduct(Product? newProduct)
        {
            if (newProduct == null)
            {
                return;
            }

            Console.WriteLine("What is the name for the Product?");
            newProduct.Name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the description for the Product?");
            newProduct.Description = Console.ReadLine() ?? string.Empty;

            double Price;
            Console.WriteLine("What is the unit price for the Product?");
            while (!double.TryParse(Console.ReadLine(), out Price)) { Console.WriteLine("Please Enter A Double"); }
            newProduct.Price = Price;

            newProduct.BG = false;


            if (newProduct is ProductByQuantity)
            {
                var newQuantity = newProduct as ProductByQuantity;

                if (newQuantity != null)
                { //only prompt for inventory quantity
                    int IQuantity;
                    Console.WriteLine("What is the number of units in the Inventory for the Product? (Quantity)");
                    while (!int.TryParse(Console.ReadLine(), out IQuantity)) { Console.WriteLine("Please Enter An Integer"); }
                    newQuantity.InventoryQuantity = IQuantity;

                    //initalize quantity as inventory 
                    newQuantity.Quantity = IQuantity;

                    //set total price to 0 as product isn't in cart
                    //newQuantity.TotalPrice = 0;
                }
            }
            else if (newProduct is ProductByWeight)
            {
                var newWeight = newProduct as ProductByWeight;

                if (newWeight != null)
                {
                    double weight;
                    Console.WriteLine("What is the number of units being purchased for the Product? (Weight)");
                    while (!double.TryParse(Console.ReadLine(), out weight)) { Console.WriteLine("Please Enter An Double"); }
                    newWeight.Weight = weight;

                    //multiply weight and price to receive total price
                    newWeight.Calculate();
                    //newWeight.TotalPrice = newWeight.Price * newWeight.Weight;
                }
            }


        }
    }
}
