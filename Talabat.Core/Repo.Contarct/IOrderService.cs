using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Repo.Contarct
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethod, Address ShippingAddress);
        Task<IEnumerable<Order>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<Order?> GetOrderByIdForUserAsync(string BuyerEmail, int orderId);
        Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodsAsync();
    }
}
