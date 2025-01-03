﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalOffice_BE.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalOffice_BE.Data;

[Table("material_types", Schema = ContextSchemaConstants.main)]
public class MaterialTypeDto : BaseTableKey<long>
{
    public string Name { get; set; } = null!;
    public ICollection<MaterialDto>? Materials { get; set; }
}

public class MaterialTypeDtoConfiguration : IEntityTypeConfiguration<MaterialTypeDto>
{
    public void Configure(EntityTypeBuilder<MaterialTypeDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name)
            .IsUnique();
        builder.Property(e => e.Id)
            .HasColumnName("id"); 
        builder.Property(e => e.Name)
            .HasColumnName("name");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");
    }
}