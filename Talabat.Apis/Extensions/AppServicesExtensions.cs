using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repo.Contarct;
using Talabat.Repo;
using Talabat.Repo.Data;
using Talabat.Repo.Repo_Impelemnt;
using TalabatService;

namespace Talabat.APIs.Extensions
{

    public static class AppServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {
            // ?? Add DbContext
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenaricRepo<>), typeof(GenaricRepo<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            //builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));
            services.AddSingleton(typeof(IBasketRepo), typeof(BasketRepo));
            services.AddAutoMapper(typeof(MappingProfile));


    


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState
                        .Where(p => p.Value.Errors.Count > 0)
                        .SelectMany(p => p.Value.Errors)
                        .Select(p => p.ErrorMessage)
                        .ToList();

                    var response = new ApiValidationErrors()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            
            });
            return services;
        }
    }
}
