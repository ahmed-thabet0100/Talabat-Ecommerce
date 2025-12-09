using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repo.Data.Config
{
    public class CatogaryConfig : IEntityTypeConfiguration<Catogary>
    {
        public void Configure(EntityTypeBuilder<Catogary> builder)
        {
            builder.Property(b => b.Name).IsRequired().HasMaxLength(128);

        }
    }
}
