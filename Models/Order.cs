using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int SuperMarketId { get; set; }
        public Customer Customer { get; set; }
        public decimal TotalCost { get; set; }
        public SuperMarket SuperMarket { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
