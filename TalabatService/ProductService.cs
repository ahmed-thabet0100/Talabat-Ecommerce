using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repo.Contarct;
using Talabat.Core.Specifications;

namespace TalabatService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpec(specParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            return products;
        }
        public async Task<Product?> GetProductByIdAsync(int ProductId)
        {
            return await _unitOfWork.Repository<Product>().GetAsync(ProductId);
        }
        public async Task<int> GetCountAsync(ProductSpecParams specParams)
        {
            var countSpec = new ProductWithFiltersForCountSpecification(specParams);
            var count = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            return count;

        }
        public async Task<IEnumerable<Brand>> GetBrandsAsync()
            => await _unitOfWork.Repository<Brand>().GetAllAsync();

        public async Task<IEnumerable<Catogary>> GetCategoryAsync()
            => await _unitOfWork.Repository<Catogary>().GetAllAsync();

    }
}
