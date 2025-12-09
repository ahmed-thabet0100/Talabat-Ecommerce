using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repo.Data.Config.OrderConfig
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, x => x.WithOwner());

            // to store enum as string in database 
            // and return value as OrderStatus (int)
            builder.Property(o => o.Status)
                .HasConversion(
                    Ostatus => Ostatus.ToString(),
                    Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)
                );
            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(o => o.Items)
          .WithOne(oi => oi.Order)
          .HasForeignKey(oi => oi.OrderId)
          .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
