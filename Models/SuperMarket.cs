using System.Collections.Generic;

namespace ShoppingSystem.Models
{
    public class SuperMarket
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
