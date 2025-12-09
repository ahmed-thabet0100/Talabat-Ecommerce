using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndCategorySpec : BaseSpecification<Product>
    {
        public ProductWithBrandAndCategorySpec(ProductSpecParams specParams) :
            base(p =>
                (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search.ToLower())) &&
                (!specParams.brandId.HasValue || p.BrandId == specParams.brandId.Value) &&
                (!specParams.categoryId.HasValue || p.CatogaryId == specParams.categoryId.Value)
            )


        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Category);

            if (!string.IsNullOrEmpty(specParams.sort))
            {
                switch (specParams.sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;

                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            ApplyPagination(skip: (specParams.PageIndex - 1) * specParams.PageSize, take: specParams.PageSize);
        }
    }
}
