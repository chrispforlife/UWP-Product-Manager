 using Library.TaskManagement.Models;

namespace ProductApplication.API.Database
{
    public static class FakeDatabase
    {

        private static List<Product> SaveI = new List<Product>();

        public static List<Product> SInventory 
        {
            get { return SaveI; }
        }

        private static List<Product> inventory = new List<Product>
        {
            new ProductByWeight{Name = "Weight", Description="PBW", Price=1, Weight = 50, IWeight= 50, CWeight=0, Id= 1},
            new ProductByQuantity{Name = "Quantity", Description="PBQ", Price=1, Quantity = 50, InventoryQuantity= 50, CartQuantity=0, Id= 2} 
        };

        public static List<Product> Inventory
        {
            get 
            {
                return inventory;
            }
        }

        public static List<Product> UpdateSI(List<Product> p)
        { //update Saved Inventory field
            SaveI = p;
            return SaveI;
        }

        public static List<Product> UpdateI(List<Product> p) 
        { //update Inventory field
            inventory = p;
            return inventory;
        }

        private static string ccname { get; set; }
        public static string CCname
        {
            get 
            {
                return ccname;
            }

            set 
            {
                ccname = value;
            }
        }

        public static List<Product> currentcart = new List<Product>();

        public static List<Product> Currentcart 
        {
            get { return currentcart; }
            set { currentcart = value; }
        }

        private static Dictionary<string, List<Product>> carts = new Dictionary<string, List<Product>>();
        public static Dictionary<string, List<Product>> Carts {
            get 
            {
                return carts;
            }
        }

        public static List<Product> addCart(string cname, List<Product> p) 
        {
            carts.Add(cname, p);
            return p;
        }

        public static List<Product> getCart(string cname)
        {
            if (carts.ContainsKey(cname))
            {
                return carts[cname];
            }
            else 
            {
                return null;
            }

        }



        public static int NextId
        {
            get 
            {
                return Inventory.Select(i => i.Id).Max() + 1;
            }
        }


    }
}
