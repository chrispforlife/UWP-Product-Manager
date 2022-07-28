using Library.TaskManagement.Models;
using ProductApplication.API.Database;

namespace ProductApplication.API.EC
{
    public class InventoryEC
    {
        public List<Product> Get()
        {
            return FakeDatabase.Inventory;
        }

        public Product AddOrUpdate(Product p)
        {
            if (p.Id <= 0)
            {
                p.Id = FakeDatabase.NextId;
                FakeDatabase.Inventory.Add(p);
            }
            else
            {
                var itemToReplace = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == p.Id);
                if (itemToReplace != null)
                {
                    FakeDatabase.Inventory.Remove(itemToReplace);
                    FakeDatabase.Inventory.Add(p);
                }
                else
                {
                    FakeDatabase.Inventory.Add(p);
                }


            }

            return p;
        }

        public ProductByQuantity AddOrUpdate(ProductByQuantity quantity)
        {
            if (quantity.Id <= 0)
            {
                quantity.Id = FakeDatabase.NextId;
                FakeDatabase.Inventory.Add(quantity);
            }

            var itemToUpdate = FakeDatabase.Inventory.FirstOrDefault(t => t.Id == quantity.Id);
            if (itemToUpdate != null)
            {
                FakeDatabase.Inventory.Remove(itemToUpdate);
                FakeDatabase.Inventory.Add(quantity);
            }
            else 
            {
                FakeDatabase.Inventory.Add(quantity);
            }

            return quantity;
        }

        public ProductByWeight AddOrUpdate(ProductByWeight weight)
        {
            if (weight.Id <= 0)
            {
                weight.Id = FakeDatabase.NextId;
                FakeDatabase.Inventory.Add(weight);
            }

            var productToUpdate = FakeDatabase.Inventory.FirstOrDefault(t => t.Id == weight.Id);
            if (productToUpdate != null)
            {
                FakeDatabase.Inventory.Remove(productToUpdate);
                FakeDatabase.Inventory.Add(weight);
            }
            else
            {
                FakeDatabase.Inventory.Add(weight);
            }

            return weight;
        }

    }
}
