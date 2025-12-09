using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using static System.Net.WebRequestMethods;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<IEnumerable<Product>, IEnumerable<ProductDtos>>();
            CreateMap<Product, ProductDtos>()
                .ForMember(p => p.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(p => p.Catogary, o => o.MapFrom(s => s.Category.Name))
                //.ForMember(p => p.PictureUrl, o => o.MapFrom(s => $"https://localhost:7052/{s.PictureUrl}"));
                .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPicturerURLResolver>());

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src=>src.PictureUrl));
            CreateMap<shipToAddress, Talabat.Core.Entities.Order_Aggregate.Address>().ReverseMap();

            CreateMap<Core.Entities.Identity.Address, AddressDTo>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LName))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Ciy))
            .ReverseMap()
            .ForMember(dest => dest.FName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Ciy, opt => opt.MapFrom(src => src.City));

            CreateMap<Order, OrderToReturnDTo>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryCost, o => o.MapFrom(o => o.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(o => o.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(o => o.Product.ProductName))
                .ForMember(d => d.PictureURL, o => o.MapFrom<OrderItemPicturerURLResolver>());
        }
    }
}
