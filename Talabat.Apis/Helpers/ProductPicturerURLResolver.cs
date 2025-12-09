using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class ProductPicturerURLResolver : IValueResolver<Product, ProductDtos, string>
    {
        public IConfiguration Config { get; }
        public ProductPicturerURLResolver(IConfiguration config)
        {
            Config = config;
        }


        public string Resolve(Product source, ProductDtos destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{Config["BaseURL"]}/{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
