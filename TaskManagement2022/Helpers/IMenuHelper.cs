using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement2022.Helpers
{
    internal class IMenuHelper
    {
        public void DoWork()
        {
            Console.WriteLine("Welcome to the Product Management App for 2022!");
            //list of inventory items

            var inventoryService = InventoryService.Current;
            var action = PrintMenu();
            var choice = 0; // used for id

            while (action != ActionType.Quit)
            {
                switch (action)
                {
                    case ActionType.Create_Inventory:
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

                        inventoryService.AddOrUpdate(Helpers.FillInventoryItem(newProduct));
                        break;
                    case ActionType.Update_Inventory:
                        choice = SelectInventoryItem("update");
                        Product? ProductToUpdate = inventoryService.Inventory.FirstOrDefault(i => i.Id == choice);

                        if (ProductToUpdate != null) 
                        {
                            Console.WriteLine($"Proceed to Update {ProductToUpdate}");
                            inventoryService.AddOrUpdate(Helpers.FillInventoryItem(ProductToUpdate));
                            Console.WriteLine("Product was successfully updated\n");
                        }
                        else 
                        { Console.WriteLine("Product was NOT successfully updated\n"); }
                        break;
                    case ActionType.Mark_BOGO:
                        choice = SelectInventoryItem("Mark BOGO");
                        var ProductToMark = inventoryService.Inventory.FirstOrDefault(i => i.Id == choice);
                        if (ProductToMark != null)
                        { 
                            inventoryService.BOGO(ProductToMark);
                            Console.WriteLine("Product was successfully marked as BOGO\n");
                        }
                        else
                        { Console.WriteLine("Product was NOT successfully updated\n"); }

                        break;

                    case ActionType.Delete_Inventory:
                        var ProductIDToDelete = SelectInventoryItem("delete");
                        inventoryService.Delete(ProductIDToDelete);
                        break;
                    case ActionType.Read_Inventory:
                        Helpers.ListItems(inventoryService.Inventory);
                        break;
                    case ActionType.Search_Inventory:
                        Console.WriteLine("Please enter your search query (Name/Description): ");
                        var slist = inventoryService.GetFilteredList(Console.ReadLine() ?? string.Empty);
                        if (slist != null && slist.Any())
                            Helpers.ListItems(slist);
                        else
                            Console.WriteLine("Search query does not exist!");
                        break;
                    case ActionType.Sort_Inventory:
                        Helpers.ListItems(inventoryService.SortI());
                        break;
                    case ActionType.Save_Inventory:
                        inventoryService.Save();
                        break;
                    case ActionType.Load_Inventory:
                        inventoryService.Load();
                        break;
                }
                action = PrintMenu();
            }
        }

        private ActionType PrintMenu()
        {
            while (true)
            {
                Console.WriteLine("\nSelection an option to begin: ");
                Console.WriteLine("1. Add product to inventory");
                Console.WriteLine("2. List products to inventory");
                Console.WriteLine("3. Search product in inventory");
                Console.WriteLine("4. Update product in inventory");
                Console.WriteLine("5. Mark product in inventory as BOGO");
                Console.WriteLine("6. Delete product from inventory");
                Console.WriteLine("7. Sort Inventory");
                Console.WriteLine("8. Save Inventory");
                Console.WriteLine("9. Load inventory");
                Console.WriteLine("0. Quit\n");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            return ActionType.Create_Inventory;
                        case 2:
                            return ActionType.Read_Inventory;
                        case 3:
                            return ActionType.Search_Inventory;
                        case 4:
                            return ActionType.Update_Inventory;
                        case 5:
                            return ActionType.Mark_BOGO;
                        case 6:
                            return ActionType.Delete_Inventory;
                        case 7:
                            return ActionType.Sort_Inventory;
                        case 8:
                            return ActionType.Save_Inventory;
                        case 9:
                            return ActionType.Load_Inventory;
                        case 0:
                            return ActionType.Quit;
                    }
                }
                Console.WriteLine();
            }
        }

        private int SelectInventoryItem(string action) 
        {
            Console.WriteLine($"\nDisplaying List of Products to {action} from");
            Helpers.ListItems(InventoryService.Current.Inventory);

            Console.WriteLine($"Which inventory item would you like to {action}?(ID)");
            var ID = int.Parse(Console.ReadLine() ?? "0");
            return ID;
        }
    }
}
