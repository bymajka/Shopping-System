using System.Collections.Generic;

namespace ShoppingSystem.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
