namespace Library.TaskManagement.Models
{
    public partial class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public int Id { get; set; }
        public bool BG { get; set; }

        public Product()
        {
            BG = false;
            TotalPrice = 0;
        }

        public override string ToString()
        {
            return $"ID:{Id} :: Name:{Name} :: BOGO:{BG} ::Description:{Description} :: Price:{Price} :: Quantity:{Quantity} :: Total Price:{TotalPrice}";
        }
    }
}
