using Library.Standard.Products.Utility;
using Library.TaskManagement.Models;
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
            /* Assignment 3 */
            inventory = new List<Product>();


            /*Assignment 4 */
            var productsJson = new WebRequestHandler().Get("http://localhost:5048/Inventory").Result;
            if (productsJson != null)
            {
                inventory = JsonConvert.DeserializeObject<List<Product>>(productsJson);
            }

        }

        public void AddOrUpdate(Product product)
        {
            //Assignment 3
            /*
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
            */

            //Assignment 4
            var response = new WebRequestHandler().Post("http://localhost:5048/Inventory/AddOrUpdate", product).Result;
            var newP = JsonConvert.DeserializeObject<Product>(response);

            var oldVersion = inventory.FirstOrDefault(i => i.Id == newP.Id);
            if (oldVersion != null)
            {
                var index = inventory.IndexOf(oldVersion);
                inventory.RemoveAt(index);
                inventory.Insert(index, newP);
                int num = inventory.Count();
            }
            else
            {
                inventory.Add(newP);
            }
        }

        public void Delete(int id)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5048/Inventory/Delete/{id}");
            var ProductToDelete = inventory.FirstOrDefault(i => i.Id == id);
            if (ProductToDelete != null)
            {
                Console.WriteLine($"Deleting {ProductToDelete}");
                inventory.Remove(ProductToDelete);
            }
        }

        public void MarkBOGO(Product p) 
        {
            p.BG = true;
            AddOrUpdate(p);
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

        /* Assignment 3 Save
        public void Save(string fileName = null)
        {
            
            //NEFARIOUS CODE
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
        */

        public void Save() 
        {
            var savecart = new WebRequestHandler().Post($"http://localhost:5048/Inventory/Save", inventory).Result;
            if (savecart != null) { inventory = JsonConvert.DeserializeObject<List<Product>>(savecart); }
        }

        /* Assignment 3 Load 
        public void Load(string fileName = null)
        {
            //NEFARIOUS CODE
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
            */

        public void Load()
        {
            var loadcart = new WebRequestHandler().Get($"http://localhost:5048/Inventory/Load").Result;
            if (loadcart != null) { inventory = JsonConvert.DeserializeObject<List<Product>>(loadcart); }
        }
    }
} 
