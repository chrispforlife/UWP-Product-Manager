using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using TaskManagement2022.Helpers;

namespace TaskManagement2022 // Note: actual namespace depends on the project name.
{
    public partial class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to the Product Management App for 2022!");
            
            var option = GetUserTypeChoice();
            while (option != UserType.None)
            {
                switch (option)
                {
                    case UserType.Employee:
                        new IMenuHelper().DoWork();
                        break;
                    case UserType.Customer:
                        new CMenuHelper().DoWork();
                        break;
                }
                option = GetUserTypeChoice();
                Console.WriteLine(option);
            }
        }
  
       private static UserType GetUserTypeChoice() 
        {
            while (true) 
            {
                Console.WriteLine("Are you a store employee or a customer?");
                Console.WriteLine("1. Employee");
                Console.WriteLine("2. Customer");
                Console.WriteLine("3. Quit");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            return UserType.Employee;
                        case 2:
                            return UserType.Customer;
                        case 3:
                            return UserType.None; 
                    }
                }
                Console.WriteLine();
            }
        }
    } 


    public enum ActionType
    {
        Create_Inventory
        , Read_Inventory
        , Update_Inventory
        , Delete_Inventory
        , Search_Inventory
        , Sort_Inventory
        , Mark_BOGO
        , Save_Inventory
        , Load_Inventory
        , Addto_Cart
        , Update_Cart
        , Read_Cart
        , Delete_Cart
        , BOGO_Cart
        , Search_Cart
        , Sort_Cart
        , Save_Cart
        , Load_Cart
        , Checkout_Cart
        , Quit
      //  Create, Read, ReadIncomplete, Update, Delete, Search, Add_to_Cart, Checkout, SortN, SortTP, SortUP, BOGO, Save, Load, Exit
    }

    public enum UserType
    {
        Employee, Customer, None
    }
}


/*            var PService = ProductService.Current;

            bool cont = true;
            while (cont)
            {
                var action = PrintMenu();

                if (action == ActionType.Create)
                {
                    Console.WriteLine("You chose to add a Product");
                    Console.WriteLine("Will you measure this product by (W)eight or (Q)uantity?");

                    char c;
                    while (!char.TryParse(Console.ReadLine(), out c) || (c != 'W' && c != 'Q')) { Console.WriteLine("Please choose W or Q "); }
                    Product? newProduct = null;

                    if (c == 'W')
                    {
                        newProduct = new ProductByWeight();
                    }
                    else if (c == 'Q')
                    {
                        newProduct = new ProductByQuantity();
                    }


                    //var newProduct = new Product();
                    FillProduct(newProduct); //enter fields for new product
                    PService.Create(newProduct); //adds to current inventory

                }
                else if (action == ActionType.Read)
                {
                    Console.WriteLine("You chose to list all Products");
                    Console.WriteLine("Would you like to List (I)nventory or (C)arts?(I/C) ");
                    char c;
                    while (!char.TryParse(Console.ReadLine(), out c) || (c != 'I' && c != 'C')) { Console.WriteLine("Please choose I or C "); }

                    if (c == 'I')
                    {
                        PService.ReadI();
                    }
                    else if (c == 'C')
                    {
                        PService.ReadC();
                    }
                }
                else if (action == ActionType.Update)
                {
                    Console.WriteLine("You chose to update a Product");
                    Console.WriteLine("Which Product would you like to update in Inventory?(Enter ID)");
                    var choice = int.Parse(Console.ReadLine() ?? "0");

                    var ProductInterest = PService.Products.FirstOrDefault(t => t.Id == choice) ?? new Product();
                    FillProduct(ProductInterest);
                    PService.Update(ProductInterest);

                }
                else if (action == ActionType.Delete)
                { //deletes product based on index
                    Console.WriteLine("You chose to delete a product");
                    PService.Delete();

                }
                else if (action == ActionType.Search)
                { //searches for Product based on Name or Description
                    Console.WriteLine("You chose to search for a product, enter a NAME or DESCRIPTION: ");
                    var query = Console.ReadLine() ?? String.Empty;
                    var Inv = PService.SearchI(query);
                    var Cart =PService.SearchC(query);
                    foreach (var i in Inv)
                    { Console.WriteLine(i);}
                    foreach (var i in Cart)
                    { Console.WriteLine(i); }
                }
                else if (action == ActionType.Add_to_Cart)
                { //adds index from inventory into cart list
                    Console.WriteLine("You chose to add a product to the cart");
                    Console.WriteLine("Which product from Inventory would you like to add (ID)?");
                    var ID = int.Parse(Console.ReadLine() ?? "0");

                    Console.WriteLine("What quantity of this product would you like to add to cart?");
                    var quantity = int.Parse(Console.ReadLine() ?? "0");
                    if (quantity <= 0) 
                    {
                        continue;
                    }

                    var Product = PService.Products.FirstOrDefault(t => t.Id == ID);
                    if (Product != null) 
                    {
                        PService.AddtoC(Product, quantity); 
                    }
                    
                }
                else if (action == ActionType.Checkout)
                {
                    Console.WriteLine("You chose to checkout cart");
                    PService.Checkout(); //shows cart subtotal and total with tax
                    cont = false; //exits program
                }
                else if (action == ActionType.SortN) 
                {
                    Console.WriteLine("You chose to Sort by Name"); 
                    Console.WriteLine("Would you like to sort (I)nventory or (C)arts by Name?(I/C) ");
                    char c;
                    while (!char.TryParse(Console.ReadLine(), out c) || (c != 'I' && c != 'C')) { Console.WriteLine("Please choose I or C "); }
                    if (c == 'I') 
                    {
                        PService.Products.Sort((x, y) => x.Name.CompareTo(y.Name));
                        //PService.Sort_Name(PService.Products); 
                    } 
                    else if (c == 'C') 
                    {
                        PService.Purchase.Sort((x, y) => x.Name.CompareTo(y.Name));
                        //PService.Sort_Name(PService.Purchase); 
                    }
                }
                else if (action == ActionType.SortTP)
                {
                    Console.WriteLine("You chose to Sort Cart by Total Price");
                    PService.Purchase.Sort((x, y) => x.TotalPrice.CompareTo(y.TotalPrice));
                }
                else if (action == ActionType.SortUP)
                {
                    Console.WriteLine("You chose to Sort Inventory by Unit Price");
                    PService.Products.Sort((x, y) => x.Price.CompareTo(y.Price));
                }
                else if (action == ActionType.BOGO)
                {
                    Console.WriteLine("You chose to give an Cart Product BOGO");
                    Console.WriteLine("Which Product would you like to implement BOGO?(ID From Cart)");
                    var ID = int.Parse(Console.ReadLine() ?? "0");

                    var Product = PService.Purchase.FirstOrDefault(t => t.Id == ID);
                    PService.BOGO(Product);

                }
                else if (action == ActionType.Save) 
                { //saves cart information in "Cart.json"
                    Console.WriteLine("You chose to save cart information");
                    PService.Save("SaveData.json");
                }
                else if (action == ActionType.Load)
                { //loads cart information in "Cart.json"
                    Console.WriteLine("You chose to load cart information");
                    PService.Load("SaveData.json");
                }
                else if (action == ActionType.Exit)
                {
                    Console.WriteLine("You chose to exit");
                    cont = false; //exits the program
                }
            } */

/*
        public static ActionType PrintMenu()
        {
            //CRUD = Create, Read, Update, and Delete
            Console.WriteLine("Selection an option to begin: ");
            Console.WriteLine("1. Add a Product");
            Console.WriteLine("2. List all Products");
            Console.WriteLine("3. Update a Product");
            Console.WriteLine("4. Delete a Product");
            Console.WriteLine("5. Search for a Product");
            Console.WriteLine("6. Add Product to Cart");
            Console.WriteLine("7. Checkout Products");
            Console.WriteLine("8. Sort by Name");
            Console.WriteLine("9. Sort Cart by Total Price");
            Console.WriteLine("10. Sort Inventory by Unit Price");
            Console.WriteLine("11. Mark Product as BOGO");
            Console.WriteLine("12. Save Cart");
            Console.WriteLine("13. Load Cart");
            Console.WriteLine("14. Exit");

            var input = int.Parse(Console.ReadLine() ?? "0");

            while (true)
            {
                switch (input)
                {
                    case 1:
                        return ActionType.Create;
                    case 2:
                        return ActionType.Read;
                    case 3:
                        return ActionType.Update;
                    case 4:
                        return ActionType.Delete;
                    case 5:
                        return ActionType.Search;
                    case 6:
                        return ActionType.Add_to_Cart;
                    case 7:
                        return ActionType.Checkout;
                    case 8:
                        return ActionType.SortN;
                    case 9:
                        return ActionType.SortTP;
                    case 10:
                        return ActionType.SortUP;
                    case 11:
                        return ActionType.BOGO;
                    case 12:
                        return ActionType.Save;
                    case 13:
                        return ActionType.Load;
                    case 14:
                        return ActionType.Exit;
                    default: 
                        continue;
                }
            }
        }
        */