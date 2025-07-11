﻿using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Name).HasMaxLength(50).IsRequired();

            builder.Property(p => p.Category).IsRequired();

            builder.Property(p => p.Description).HasMaxLength(200);

            builder.Property(p => p.ImageFile).HasMaxLength(100);

            builder.Property(p => p.Price).IsRequired();

        }
    }
}
