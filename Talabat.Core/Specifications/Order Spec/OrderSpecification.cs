using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderSpecification:BaseSpecification<Order>
    {
        public OrderSpecification(string buyerEmail)
            :base(o=>o.BuyerEmail==buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderSpecification(string buyerEmail, int orderId) 
            : base(o => o.BuyerEmail== buyerEmail && o.Id == orderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
        public OrderSpecification(string paymentIntentId, double? num = 0)
            :base(o=> o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
