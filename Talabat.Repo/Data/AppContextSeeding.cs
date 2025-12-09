using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repo.Data
{
    public static class AppContextSeeding
    {
        public async static Task SeedAsync(AppDbContext _dbcontext)
        {
            if (_dbcontext.Catogaries.Count() == 0)
            {
                var CategoryData = File.ReadAllText("E:\\Asp.Net(route)\\Talabat.PROg\\Talabat.Repo\\Data\\SeedingData\\categories.json");
                var Category = JsonSerializer.Deserialize<List<Catogary>>(CategoryData);

                if (Category?.Count() > 0)
                {
                    foreach (var category in Category)
                    {
                        _dbcontext.Set<Catogary>().Add(category);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            if (_dbcontext.Brands.Count() == 0)
            {
                var BrandsData = File.ReadAllText("E:\\Asp.Net(route)\\Talabat.PROg\\Talabat.Repo\\Data\\SeedingData\\brands.json");
                var Brands = JsonSerializer.Deserialize<List<Brand>>(BrandsData);

                if (Brands?.Count() > 0)
                {
                    foreach (var brand in Brands)
                    {
                        _dbcontext.Set<Brand>().Add(brand);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            if (_dbcontext.Products.Count() == 0)
            {
                var ProductsData = File.ReadAllText("E:\\Asp.Net(route)\\Talabat.PROg\\Talabat.Repo\\Data\\SeedingData\\products.json");
                var Product = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (Product?.Count() > 0)
                {
                    foreach (var product in Product)
                    {
                        _dbcontext.Set<Product>().Add(product);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            if (_dbcontext.DeliveryMethod.Count() == 0)
            {
                var MethodsData = File.ReadAllText("E:\\Asp.Net(route)\\Talabat.PROg\\Talabat.Repo\\Data\\SeedingData\\delivery.json");
                var Methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodsData);

                if (Methods?.Count() > 0)
                {
                    foreach (var method in Methods)
                    {
                        _dbcontext.Set<DeliveryMethod>().Add(method);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }


        }
    }
}
