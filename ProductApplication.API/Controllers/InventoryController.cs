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
        private InventoryEC IEC;

        public InventoryController(ILogger<InventoryController> logger)
        {
            IEC = new InventoryEC();
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
        }

        [HttpGet("Delete/{id}")]
        public Product Remove(int id)
        {
            var prodToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.Id == id);

            FakeDatabase.Inventory.Remove(prodToDelete);
            return prodToDelete ?? new Product();
        }


        [HttpPost("Save")]
        public List<Product> Save(List<Product> inventory)
        {
            List<Product> temp = FakeDatabase.UpdateSI(inventory);
            return temp;
        }


        [HttpGet("Load")]
        public List<Product> Load()
        {
            return FakeDatabase.UpdateI(FakeDatabase.SInventory);
        }
    }
}
