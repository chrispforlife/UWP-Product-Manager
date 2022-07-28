using Library.TaskManagement.Models;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.API.EC;
using ProductApplication.API.Controllers;
using ProductApplication.API.Database;

namespace ProductApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private CartEC CEC;

        public CartController(ILogger<CartController> logger)
        {
            CEC = new CartEC();
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return new CartEC().Get();
        }

        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product p)
        {
            if (p is ProductByWeight)
            {
                var w = p as ProductByWeight;
                return CEC.AddOrUpdate(w);
            }
            else if (p is ProductByQuantity)
            {
                var q = p as ProductByQuantity;
                return CEC.AddOrUpdate(q);
            }
            else
            {
                return CEC.AddOrUpdate(p);
            }
        }

        [HttpGet("Delete/{id}")]
        public Product Remove(int id)
        {
            var prodToDelete = FakeDatabase.Currentcart.FirstOrDefault(i => i.Id == id);

            FakeDatabase.Currentcart.Remove(prodToDelete);

            return prodToDelete ?? new Product();
        }

        [HttpGet("Save/{name}")]
        public List<Product> Save(string name)
        {

            List<Product> Carts = FakeDatabase.Currentcart.ToList();
            FakeDatabase.CCname = name;
            FakeDatabase.addCart(FakeDatabase.CCname, Carts);
            return Carts;
        }

        [HttpGet("Load/{name}")]

        public List<Product> Load(string name)
        { 
            FakeDatabase.Currentcart = FakeDatabase.getCart(name);
            return FakeDatabase.Currentcart;
        }

        [HttpPost("SaveCarts")]
        public List<string> SaveCarts(List<string> cartnames)
        {
            return cartnames;
        }

        [HttpGet("LoadCarts")]
        public List<string> LoadCarts()
        {
            var cartnames = new List<string>();
            foreach (var cn in FakeDatabase.Carts) 
            {
                cartnames.Add(cn.Key);
            }
            return cartnames;
        }
    }
}
