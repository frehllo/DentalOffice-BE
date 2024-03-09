using DentalOffice_BE.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalOffice_BE.Data;

[Table("studios", Schema = ContextSchemaConstants.main)]
public class StudioDto : BaseTableKey<long>
{
    public string Name { get; set; } = null!;
    public string? Color { get; set; } = "#ffffff";
}

public class StudioDtoConfiguration : IEntityTypeConfiguration<StudioDto>
{
    public void Configure(EntityTypeBuilder<StudioDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name)
            .IsUnique();
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Color)
            .HasColumnName("color")
            .HasMaxLength(7);
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
