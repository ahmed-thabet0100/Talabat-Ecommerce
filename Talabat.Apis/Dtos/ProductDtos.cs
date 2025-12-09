using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
    public class ProductDtos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public string Brand { get; set; }
        public int BrandId { get; set; }
        [JsonPropertyName("Category")]
        public string Catogary { get; set; }
        [JsonPropertyName("CategoryId")]
        public int CatogaryId { get; set; }

    }
}
