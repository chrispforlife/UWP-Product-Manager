using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;

namespace Library.TaskManagement.Services
{
    public class InventoryService
    {
        private string _persistPath = "inventory.json";
        private static InventoryService? instance;


        public static InventoryService Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new InventoryService();
                }
                return instance;
            }
        }

        private List<Product> inventory;
        public List<Product> Inventory
        {
            get { return inventory; }
        }
        private InventoryService()
        {
            inventory = new List<Product>();
        }

        public void AddOrUpdate(Product product)
        {
            Console.WriteLine($"Current product is {product}");

            if (product.Id == 0)
            {
                if (inventory.Any())
                { //if list has elements add to product id
                    product.Id = inventory.Select(i => i.Id).Max() + 1;
                }
                else
                { //if first element make id 1
                    product.Id = 1;
                }
            }

            if (!inventory.Any(i => i.Id == product.Id))
            { //not in inventory add item
                inventory.Add(product);
            }
        }

        public void Delete(int id)
        {
            var ProductToDelete = inventory.FirstOrDefault(i => i.Id == id);
            if (ProductToDelete != null)
            {
                Console.WriteLine($"Deleting {ProductToDelete}");
                inventory.Remove(ProductToDelete);
            }
        }

        public IEnumerable<Product> SortI()
        {
            Console.WriteLine("Would you like to sort Inventory by (N)ame or by (U)nit Price?(N/U) ");
            char c;
            while (!char.TryParse(Console.ReadLine(), out c) || (c != 'N' && c != 'U')) { Console.WriteLine("Please choose N or U "); }
            switch (c) 
            {
                case 'N':
                    Console.WriteLine("Sorted by Name");
                    inventory = inventory.OrderBy(i => i.Name).ToList();
                    break;
                case 'U':
                    Console.WriteLine("Sorted by Price");
                    inventory = inventory.OrderBy(i => i.Price).ToList();
                    break;
            }
            return inventory;
        }

        public IEnumerable<Product> GetFilteredList(string? query)
        { //searches for product based on name and description
            if (string.IsNullOrEmpty(query))
            {
                return Inventory;
            }
            return Inventory.Where(i =>
            (i?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
            || (i?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }

        public void BOGO(Product p)
        {p.BG = true;}

        public void Save()
        { //NEFARIOUS CODE
            var ProductJson = JsonConvert.SerializeObject(inventory
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(_persistPath, ProductJson);
        }

        public void Load()
        {  //NEFARIOUS CODE
            var ProductJson = File.ReadAllText(_persistPath);
            inventory = JsonConvert.DeserializeObject<List<Product>>
                (ProductJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();
        }
    }
}
