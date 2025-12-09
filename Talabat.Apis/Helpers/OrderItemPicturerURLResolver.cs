using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPicturerURLResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration Config;

        public OrderItemPicturerURLResolver(IConfiguration config )
        {
            Config = config;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureURL))
            {
                return $"{Config["BaseURL"]}/{source.Product.PictureURL}";
            }
            return string.Empty;

        }
    }
}
