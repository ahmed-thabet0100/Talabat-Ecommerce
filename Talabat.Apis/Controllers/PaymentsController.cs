using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repo.Contarct;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public PaymentsController(IPaymentService paymentService,
            IMapper mapper ,
            IConfiguration config)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateORupdatePaymednt(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null) return BadRequest(new ApiRespone(400, "An Error With Your Basket"));
            return Ok(_mapper.Map<CustomerBasket,CustomerBasketDto>(basket));
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _config["StripeSettings:WebhookSecret"]
            );

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            Order order = null;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                    break;

                case "payment_intent.payment_failed":
                    await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                    break;
            }
            return Ok();
        }
    }
}
