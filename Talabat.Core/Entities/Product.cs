using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseClass
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public Brand Brand { get; set; }
        [InverseProperty(nameof(Brand))]
        public int BrandId { get; set; }

        public Catogary Category { get; set; }
        [InverseProperty(nameof(Category))]
        public int CatogaryId { get; set; }
    }
}
