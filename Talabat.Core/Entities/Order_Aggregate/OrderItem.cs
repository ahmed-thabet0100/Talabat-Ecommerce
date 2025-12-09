using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class OrderItem : BaseClass
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOrder product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }

        public ProductItemOrder Product { get; set; } //[one 2 one]
        public int Quantity { get; set; }
        public decimal Price { get; set; }  // price after sale & discount

        // FK
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation property
    }
}
