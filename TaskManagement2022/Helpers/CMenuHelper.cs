using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement2022.Helpers
{
    internal class CMenuHelper
    {
        public void DoWork()
        {
            Console.WriteLine("Welcome to the Product Management App for 2022!");
            //list of carts items

            var cartService = ProductService.Current;
            var action = PrintMenu();
            var choice = 0; //used for id
            Boolean checkout = false;

            while (action != ActionType.Quit && checkout == false) 
            {
                switch (action)
                {
                    case ActionType.Addto_Cart:
                        Helpers.ListItems(cartService.Products);
                        Console.WriteLine("What is the ID of the inventory Product you are adding to cart?");
                        choice = SelectCartItem("Add to cart");

                        var Product = cartService.Products.FirstOrDefault(t => t.Id == choice);
                        if (Product != null)
                        {
                            cartService.AddtoC(Product);
                        }
                        break;
                    case ActionType.Read_Cart:
                        Helpers.ListItems(cartService.Carts);
                        break;
                    case ActionType.Search_Cart:
                        Console.WriteLine("Please enter your search query (Name/Description): ");
                        var slist = cartService.GetFilteredList(Console.ReadLine() ?? string.Empty);
                        if (slist != null && slist.Any())
                            Helpers.ListItems(slist);
                        else
                            Console.WriteLine("Search query does not exist!");
                        break;
                    case ActionType.Sort_Cart:
                        cartService.SortC();
                        break;
                    case ActionType.Update_Cart:
                        choice = SelectCartItem("Update");
                        Product? ProductToUpdate = cartService.Carts.FirstOrDefault(i => i.Id == choice);

                        if (ProductToUpdate != null)
                        {
                            Console.WriteLine($"Proceed to Update {ProductToUpdate}");
                            Helpers.FillInventoryItem(ProductToUpdate);
                            Console.WriteLine("Product was successfully updated\n");
                        }
                        else
                        { Console.WriteLine("Product was NOT successfully updated\n"); }
                        break;
                     
                    case ActionType.Delete_Cart:
                        var ProductIDToDelete = SelectCartItem("delete");
                        cartService.Delete(ProductIDToDelete);
                        break;

                    case ActionType.Save_Cart:
                        cartService.Save();  
                        break;
                    case ActionType.Load_Cart:
                        cartService.Load();
                        break;
                    case ActionType.Checkout_Cart:
                        cartService.Checkout();
                        checkout = true;
                        break;
                }
                if (checkout != true)
                    action = PrintMenu();
            }
        }

        public static ActionType PrintMenu()
        {
            while (true)
            {
                Console.WriteLine("\nSelect from the following options:");
                Console.WriteLine("1. Add product to cart");
                Console.WriteLine("2. List products in cart");
                Console.WriteLine("3. Search product in cart");
                Console.WriteLine("4. Sort products in cart");
                Console.WriteLine("5. Update product in cart");
                Console.WriteLine("6. Delete product from cart");
                Console.WriteLine("7. Save cart");
                Console.WriteLine("8. Load cart");
                Console.WriteLine("9. Checkout cart");
                Console.WriteLine("0. Quit\n");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            return ActionType.Addto_Cart;
                        case 2:
                            return ActionType.Read_Cart;
                        case 3:
                            return ActionType.Search_Cart;
                        case 4:
                            return ActionType.Sort_Cart;
                        case 5:
                            return ActionType.Update_Cart;
                        case 6:
                            return ActionType.Delete_Cart;
                        case 7:
                            return ActionType.Save_Cart;
                        case 8:
                            return ActionType.Load_Cart;
                        case 9:
                            return ActionType.Checkout_Cart;
                        case 0:
                            return ActionType.Quit;
                    }
                }
                Console.WriteLine();
            }
        }

        private int SelectCartItem(string action)
        {
            Console.WriteLine($"\nDisplaying List of Products to {action} from");
            Helpers.ListItems(ProductService.Current.Carts);

            Console.WriteLine($"Which inventory item would you like to {action}?(ID)");
            var ID = int.Parse(Console.ReadLine() ?? "0");
            return ID;
        }
    }
}

