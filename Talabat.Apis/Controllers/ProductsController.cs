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
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(
            IProductService productService,
            IGenaricRepo<Product> Product_Repo,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _productService = productService;
            _product_Repo = Product_Repo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region Product

        [HttpPost]
        public async Task<ActionResult<AddProductDtos>> GreateProduct([FromForm] AddProductDtos product)
        {
            product.PictureUrl = DocumentSetting.UploadFile(product.UploadPicture, "products");
            var added = _unitOfWork.Repository<Product>().AddAsync(_mapper.Map<Product>(product));
            if (!added.IsCompletedSuccessfully)
                return BadRequest(new ApiRespone(400));
            return Ok(product);
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

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var repo = _unitOfWork.Repository<Product>();
            var product = repo.GetAsync(id);
            if (product == null)
                return BadRequest(new ApiRespone(400));
            DocumentSetting.DeleteFile("images",product.Result.PictureUrl);
            var result = repo.Remove(product.Result);
            if (!result.IsCompletedSuccessfully)
            {
                return BadRequest(new ApiRespone(400));
            }
            return Ok("deleted successfully");
        }
        #endregion

        #region Category

        [HttpPost("Category")]
        public async Task<ActionResult> CreateCategory(CategoryDto category)
        {
            var result = _unitOfWork.Repository<Catogary>().AddAsync(_mapper.Map<Catogary>(category));
            if (!result.IsCompletedSuccessfully)
                return BadRequest(new ApiRespone(400));
            return Ok("Added Successfully");
        }

        [HttpGet("Categories")]
        public async Task<ActionResult<IReadOnlyList<Catogary>>> GetCategories()
        {

            var categories = await _productService.GetCategoryAsync();
            return Ok(categories);

        }
        [HttpDelete("Category")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var repo = _unitOfWork.Repository<Catogary>();
            var catogary = repo.GetAsync(id);
            if (catogary == null)
                return BadRequest(new ApiRespone(400));
            var result = repo.Remove(catogary.Result);
            if (!result.IsCompletedSuccessfully)
            {
                return BadRequest(new ApiRespone(400));
            }
            return Ok("deleted successfully");
        }

        #endregion

        #region Brand
        [HttpPost("Brand")]
        public async Task<ActionResult> CreateBrand(BrandDto brand)
        {
            var result = _unitOfWork.Repository<Brand>().AddAsync(_mapper.Map<Brand>(brand));
            if (!result.IsCompletedSuccessfully)
                return BadRequest(new ApiRespone(400));
            return Ok("Added Successfully");
           
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<Brand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpDelete("Brand")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var repo = _unitOfWork.Repository<Brand>();
            var brand = repo.GetAsync(id);
            if (brand == null)
                return BadRequest(new ApiRespone(400));
            var result = repo.Remove(brand.Result);
            if (!result.IsCompletedSuccessfully)
            {
                return BadRequest(new ApiRespone(400));
            }
            return Ok("deleted successfully");
        }
        #endregion


    }
}
