using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repo.Contarct
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams);
        Task<Product?> GetProductByIdAsync(int ProductId);
        Task<int> GetCountAsync(ProductSpecParams specParams);
        Task<IEnumerable<Brand>> GetBrandsAsync();
        Task<IEnumerable<Catogary>> GetCategoryAsync();
    }
}
