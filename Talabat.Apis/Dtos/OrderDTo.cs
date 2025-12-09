using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderDTo
    {
        public string BasketId { get; set; }
        public shipToAddress shipToAddress { get; set; }
        public int DeliveryMethodId { get; set; }  
    }
}
