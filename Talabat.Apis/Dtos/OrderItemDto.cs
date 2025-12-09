using System.Text.Json.Serialization;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [JsonPropertyName("PictureUrl")]
        public string PictureURL { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  // price after sale & discount

    }
}