using Library.TaskManagement.Models;
using ProductApplication.API.Database;

namespace ProductApplication.API.EC
{
    public class InventoryEC
    {
        public List<Product> Get()
        {
            //return FakeDatabase.Inventory;
            return Filebase.Current.Inventory;
        }

        public Product AddOrUpdate(Product p)
        {
            /* Assignment 4
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
            */

            if (p is ProductByQuantity)
            {
                return Filebase.Current.IAddOrUpdate(p) as ProductByQuantity ?? new ProductByQuantity();
            }
            else
                if (p is ProductByWeight)
            {
                return Filebase.Current.IAddOrUpdate(p) as ProductByWeight ?? new ProductByWeight();
            }
            else
            {
                return Filebase.Current.IAddOrUpdate(p) as Product ?? new Product();
            }
        }

        public bool Delete(int id)
        {
            if (Filebase.Current.Delete(id))
            {
                /*
                var prodToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == id);

                FakeDatabase.Inventory.Remove(prodToDelete);
                return prodToDelete ?? new Product();
                */
                return true;
            }

            return false;
        }

        public List<Product> Save() 
        {
            /*
            List<Product> temp = FakeDatabase.UpdateSI(inventory);
            return temp;
            */
            return Filebase.Current.SaveI();
        }

        public List<Product> Load()
        {
            return Filebase.Current.LoadI();
        }
    }
}
