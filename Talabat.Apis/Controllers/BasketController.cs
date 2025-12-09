using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repo.Contarct;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepo basketRepo, IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = new CustomerBasket(basketDto.Id);

            basket.Items = basketDto.Items.Select(i => new BasketItem
            {
                Id = i.Id,
                ProductName = i.ProductName,
                PictureUrl = i.PictureUrl,
                Price = i.Price,
                Category = i.Category,
                Brand = i.Brand,
                Quantity = i.Quantity
            }).ToList();

            basket.DeliveryMethodId = basketDto.deliveryMethodId;
            var updatedBasket = await _basketRepo.UpdateBasketAsync(basket);

            if (updatedBasket is null)
                return BadRequest(new ApiRespone(400));

            return Ok(_mapper.Map<CustomerBasketDto>(updatedBasket));
        }
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepo.DeleteBAsketAsync(id);
        }
    }
}
