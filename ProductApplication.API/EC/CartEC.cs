using Library.TaskManagement.Models;
using ProductApplication.API.Database;

namespace ProductApplication.API.EC
{
    public class CartEC
    {
        public List<Product> Get() 
        {
            return FakeDatabase.Currentcart;
        }

        public Product AddOrUpdate(Product p)
        {
            if (p.Id <= 0)
            {
                p.Id = FakeDatabase.NextId;
                FakeDatabase.Currentcart.Add(p);
            }
            else
            {
                var itemToReplace = FakeDatabase.Currentcart.FirstOrDefault(i => i.Id == p.Id);
                if (itemToReplace != null)
                {
                    FakeDatabase.Currentcart.Remove(itemToReplace);
                    FakeDatabase.Currentcart.Add(p);
                }
                else
                {
                    FakeDatabase.Currentcart.Add(p);
                }
            }

            return p;
        }

        public ProductByQuantity AddOrUpdate(ProductByQuantity quantity)
        {
            if (quantity.Id <= 0)
            {
                quantity.Id = FakeDatabase.NextId;
                FakeDatabase.Currentcart.Add(quantity);
            }

            var itemToUpdate = FakeDatabase.Currentcart.FirstOrDefault(t => t.Id == quantity.Id);
            if (itemToUpdate != null)
            {
                FakeDatabase.Currentcart.Remove(itemToUpdate);
                FakeDatabase.Currentcart.Add(quantity);
            }
            else
            {
                FakeDatabase.Currentcart.Add(quantity);
            }

            return quantity;
        }

        public ProductByWeight AddOrUpdate(ProductByWeight weight)
        {
            if (weight.Id <= 0)
            {
                weight.Id = FakeDatabase.NextId;
                FakeDatabase.Currentcart.Add(weight);
            }

            var productToUpdate = FakeDatabase.Currentcart.FirstOrDefault(t => t.Id == weight.Id);
            if (productToUpdate != null)
            {
                FakeDatabase.Currentcart.Remove(productToUpdate);
                FakeDatabase.Currentcart.Add(weight);
            }
            else
            {
                FakeDatabase.Currentcart.Add(weight);
            }

            return weight;
        }
    }
}
