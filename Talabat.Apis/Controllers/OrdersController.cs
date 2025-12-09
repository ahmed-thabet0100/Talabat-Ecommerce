using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repo.Contarct;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(Core.Entities.Order_Aggregate.Order), 200)]
        [ProducesResponseType(typeof(ApiRespone), 400)]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTo>> CreateOrder(OrderDTo orderDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var shippingAddress = _mapper.Map<shipToAddress, Talabat.Core.Entities.Order_Aggregate.Address>(orderDto.shipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDto.BasketId, orderDto.DeliveryMethodId, shippingAddress);

            if (order == null) return BadRequest(new ApiRespone(400));

            return Ok(_mapper.Map<Core.Entities.Order_Aggregate.Order, OrderToReturnDTo>(order));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTo>>> GetOrderByEmail()
        {
            var orders = await _orderService.GetAllOrdersForUserAsync(User.FindFirstValue(ClaimTypes.Email));
            return Ok(_mapper.Map<IEnumerable<Core.Entities.Order_Aggregate.Order>, IEnumerable<OrderToReturnDTo>>(orders));
        }

        [ProducesResponseType(typeof(Core.Entities.Order_Aggregate.Order), 200)]
        [ProducesResponseType(typeof(ApiRespone), 400)]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderToReturnDTo>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdForUserAsync(User.FindFirstValue(ClaimTypes.Email), orderId);
            if (order == null) return BadRequest(new ApiRespone(400));
            return Ok(_mapper.Map<Core.Entities.Order_Aggregate.Order, OrderToReturnDTo>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetAllDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
    } 
}
