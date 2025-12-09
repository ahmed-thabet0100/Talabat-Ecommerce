using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string? PictureUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue ,ErrorMessage ="select correct price")]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; } = "general";
        [Required]
        public string Brand { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Select atleast One Item")]
        public int Quantity { get; set; }

    }
}