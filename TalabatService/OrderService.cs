using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repo.Contarct;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Talabat.Core.Specifications.Order_Spec;
using System.ComponentModel.DataAnnotations;

namespace TalabatService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepo basketRepo,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int deliveryMethodId, Address ShippingAddress)
        {
            // 1- Get Product from Basket
            var basket = await _basketRepo.GetBasketAsync(BasketId);

            // 2- Get Selected Product From BasketRepo To ProductRepo
            var orderitems = new List<OrderItem>();

            if (basket?.Items?.Count>0)
            {
                var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var Product = await productRepo.GetAsync(item.Id);

                    var productItemOrder = new ProductItemOrder(item.Id, Product.Name, Product.PictureUrl);
                    
                    var orderItem = new OrderItem(productItemOrder,item.Quantity, Product.Price);
                    
                    orderitems.Add(orderItem);
                }

            }

            // 3-Calculate SubTotal
            var SubTotal = orderitems.Sum(OI => OI.Price * OI.Quantity);

            // 4-Get DeliveryMethod From Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            // validate unique PaymentIntentId for every order
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(basket.PaymentIntentId, 1.2);
            var existorder = await orderRepo.GetEntityWithSpecAsync(spec);
            if (existorder != null)
            {
                orderRepo.Delete(existorder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            // 5-Create Order
            var order = new Order(BuyerEmail, ShippingAddress, deliveryMethod, orderitems, SubTotal,basket.PaymentIntentId);

            await orderRepo.AddAsync(order);


            // 6- Save To Db
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orderRepo =  _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(BuyerEmail);
            var orders = await orderRepo.GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string BuyerEmail, int orderId)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecification(BuyerEmail, orderId);

            var order = await orderRepo.GetEntityWithSpecAsync(spec);
            return order;
        }

        public async Task<IEnumerable<DeliveryMethod>> GetAllDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        

    }
}