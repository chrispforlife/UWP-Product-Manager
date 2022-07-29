using Library.TaskManagement.Models;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.API.Controllers;
using ProductApplication.API.Database;
using ProductApplication.API.EC;

namespace ProductApplication.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        //private InventoryEC IEC;

        public InventoryController(ILogger<InventoryController> logger)
        {
            //IEC = new InventoryEC();
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return new InventoryEC().Get();
        }

        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product p)
        {
            return new InventoryEC().AddOrUpdate(p);

            /*
            if (p is ProductByWeight)
            {
                var w = p as ProductByWeight;
                return IEC.AddOrUpdate(w);
            }
            else if (p is ProductByQuantity)
            {
                var q = p as ProductByQuantity;
                return IEC.AddOrUpdate(q);
            }
            else
            {
                return IEC.AddOrUpdate(p);
            }
            */
        }

        [HttpGet("Delete/{id}")]
        public bool Remove(int id)
        { //moved original remove to EC file

            return new InventoryEC().Delete(id);
        }


        [HttpPost("Save")]
        public List<Product> Save(List<Product> inventory)
        {
            return new InventoryEC().Save();
        }


        [HttpGet("Load")]
        public List<Product> Load()
        {
            return new InventoryEC().Load();
        }
    }
}
