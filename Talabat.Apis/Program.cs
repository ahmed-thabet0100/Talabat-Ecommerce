
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repo.Contarct;
using Talabat.Repo.Data;
using Talabat.Repo.Identity;
using Talabat.Repo.Identity.SeedingData;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region container services

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            // Add redis (BasketDb)
            builder.Services.AddSingleton<IConnectionMultiplexer>((servicesprovider) =>
            {
                var connction = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connction);
            });

            // create function instead of busy
            builder.Services.AddApplicationServices(builder.Configuration);

            // create function instead of busy
            builder.Services.AddIdentityServices(builder.Configuration);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy => policy
                        .WithOrigins("http://localhost:4200", "https://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                    );
            });


            #endregion

            var app = builder.Build();

            #region auto migration
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbcontext = services.GetRequiredService<AppDbContext>();
            var dbcontextIdentity = services.GetRequiredService<AppIdentityDbContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await dbcontext.Database.MigrateAsync();
                await AppContextSeeding.SeedAsync(dbcontext);
                await dbcontextIdentity.Database.MigrateAsync();

                var usermanager = services.GetRequiredService<UserManager<AppUser>>();
                await IdentityContextSeeding.GetDataAsync(usermanager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occur during apply migration");
            }
            finally { scope.Dispose(); }
            #endregion

            // Configure the HTTP request pipeline.
            #region configration
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAngular");

            app.UseRouting();

            app.UseAuthentication();   // BEFORE Authorization
            app.UseAuthorization();

            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.MapControllers();      // آخر حاجة قبل Run

            #endregion
            app.Run();
        }

    }
}
