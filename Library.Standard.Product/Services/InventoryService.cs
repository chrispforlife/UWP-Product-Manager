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
    public class InventoryService
    {
        private string persistPath 
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private static InventoryService instance;
        private List<Product> inventory;
        public List<Product> Inventory
        {
            get { return inventory; }
        }

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
            { //not in inventory add product
                if (product is ProductByQuantity) 
                {
                    var q = product as ProductByQuantity;
                    q.UpdateI();
                    product = q;
                }
                else if (product is ProductByWeight) 
                {
                    var w = product as ProductByWeight;
                    w.UpdateI();
                    product = w;
                }
                
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

        public IEnumerable<Product> SortIBP()
        {
            Console.WriteLine("Sorted by Name");
            inventory = inventory.OrderBy(i => i.Price).ToList();
            return inventory;
        }
        public IEnumerable<Product> SortIBN()
        {
            Console.WriteLine("Sorted by Name");
            inventory = inventory.OrderBy(i => i.Name).ToList();
            return inventory;
        }

        public void Save(string fileName = null)
        { //NEFARIOUS CODE
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }
            var todosJson = JsonConvert.SerializeObject(inventory
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
                fileName = $"{persistPath}\\{fileName}.json";
            }

            var ProductJson = File.ReadAllText(fileName);
            inventory = JsonConvert.DeserializeObject<List<Product>>
                (ProductJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();
        }
    }
}
