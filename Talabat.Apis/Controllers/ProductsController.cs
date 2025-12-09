using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repo.Contarct;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IGenaricRepo<Product> _product_Repo;
        private readonly IMapper _mapper;
        private readonly IGenaricRepo<Catogary> _catogary_Repo;
        private readonly IGenaricRepo<Brand> _brand_Repo;

        public ProductsController(
            IProductService productService,
            IGenaricRepo<Product> Product_Repo,
            IMapper mapper,
            IGenaricRepo<Catogary> Catogary_Repo,
            IGenaricRepo<Brand> Brand_Repo
            )
        {
            _productService = productService;
            _product_Repo = Product_Repo;
            _mapper = mapper;
            _catogary_Repo = Catogary_Repo;
            _brand_Repo = Brand_Repo;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<IReadOnlyList<ProductDtos>>>> GetProducts([FromQuery]ProductSpecParams specParams)
        {
            var products = await _productService.GetProductsAsync(specParams);

            var count = await _productService.GetCountAsync(specParams);
            var data = _mapper.Map<IReadOnlyList<ProductDtos>>(products);
            return Ok(new Pagination<ProductDtos>(specParams.PageSize,specParams.PageIndex,count,data));
        }

        [ProducesResponseType(typeof(ProductDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiRespone), StatusCodes.Status404NotFound)]
        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductDtos>> GetById(int Id)
        {
            var product = await _productService.GetProductByIdAsync(Id);

            if (product == null)
                return NotFound(new ApiRespone(404));

            return Ok(_mapper.Map<Product, ProductDtos>(product));
        }
        [HttpGet("Categories")]
        public async Task<ActionResult<IReadOnlyList<Catogary>>> GetCategories()
        {

            var categories = await _productService.GetCategoryAsync();
            return Ok(categories);

        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

    }
}
