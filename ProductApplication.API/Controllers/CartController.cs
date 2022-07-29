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
        //private CartEC CEC;

        public CartController(ILogger<CartController> logger)
        {
            //CEC = new CartEC();
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
            return new CartEC().AddOrUpdate(p);
            /*
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
            */
        }

        [HttpGet("Delete/{id}")]
        public bool Remove(int id)
        {
            return new CartEC().Delete(id);
        }

        [HttpGet("Save/{name}")]
        public List<Product> Save(string name)
        {
            return new CartEC().Save(name);
        }

        [HttpGet("Load/{name}")]

        public List<Product> Load(string name)
        {
            return new CartEC().Load(name);
        }

        [HttpPost("SaveCarts")]
        public List<string> SaveCarts(List<string> cartnames)
        {
            return new CartEC().SaveCarts(cartnames);
        }

        [HttpGet("LoadCarts")]
        public List<string> LoadCarts()
        {
            return new CartEC().LoadCarts();
        }
    }
}
