using Library.TaskManagement.Models;
using ProductApplication.API.Database;

namespace ProductApplication.API.EC
{
    public class CartEC
    {
        public List<Product> Get() 
        {
            return Filebase.Current.Cart;
            //return FakeDatabase.Currentcart;
        }

        public Product AddOrUpdate(Product p)
        {
            /*
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
            */
            if (p is ProductByQuantity)
            {
                return Filebase.Current.CAddOrUpdate(p) as ProductByQuantity ?? new ProductByQuantity();
            }
            else
            if (p is ProductByWeight)
            {
                return Filebase.Current.CAddOrUpdate(p) as ProductByWeight ?? new ProductByWeight();
            }
            else
            {
                return Filebase.Current.CAddOrUpdate(p) as Product ?? new Product();
            }
        }

        public bool Delete(int id) 
        {
            if (Filebase.Current.Delete(id))
            {
                return true;
            }

            return false;

            /* 
            var prodToDelete = FakeDatabase.Currentcart.FirstOrDefault(i => i.Id == id);

            FakeDatabase.Currentcart.Remove(prodToDelete);

            return prodToDelete ?? new Product();
            */
        }

        public List<Product> Save(string name)
        {
            return Filebase.Current.SaveC(name);
            /* Assignment 4 
            List<Product> Carts = FakeDatabase.Currentcart.ToList();
            FakeDatabase.CCname = name;
            FakeDatabase.addCart(FakeDatabase.CCname, Carts);

            return Carts;
            */
        }

        public List<Product> Load(string name)
        {
            return Filebase.Current.LoadC(name);
            /* Assignment 4
            FakeDatabase.Currentcart = FakeDatabase.getCart(name);
            return FakeDatabase.Currentcart;
            */

        }

        public List<string> SaveCarts(List<string> cartnames)
        {
            return cartnames;
        }

        public List<string> LoadCarts()
        {
            return Filebase.Current.LoadCarts();
        }
    }
}
