using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repo.Contarct;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Order_Spec;

namespace TalabatService
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;

        public PaymentService(IBasketRepo basketRepo,
            IUnitOfWork unitOfWork,
            IConfiguration configuration)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            this.configuration = configuration;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket == null) return null;
            var _ProductRepo =  _unitOfWork.Repository<Talabat.Core.Entities.Product>();

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;

            }

            if (basket?.Items?.Count>0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _ProductRepo.GetAsync(item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // create paymentIntent
            {
                var CeateOption = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice*100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(CeateOption);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else    // update paymentIntent
            {
                var UpdateOption = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)basket.ShippingPrice*100,
                };
                paymentIntent= await paymentIntentService.UpdateAsync(basket.PaymentIntentId, UpdateOption);
            }
            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string PaymentIntentId, bool IsSucceeded)
        {
            var spec = new OrderSpecification(PaymentIntentId, 1.2);
            var order = await  _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if (IsSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.CompleteAsync();
            return order;

        }
    }
}
