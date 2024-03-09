using DentalOffice_BE.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalOffice_BE.Data;

[Table("semiproducts", Schema = ContextSchemaConstants.main)]
public class SemiProductDto : BaseTableKey<long>
{
    public string Name { get; set; } = null!;
    public ICollection<ProcessDto>? Processs { get; set; }
}

public class SemiProductDtoConfiguration : IEntityTypeConfiguration<SemiProductDto>
{
    public void Configure(EntityTypeBuilder<SemiProductDto> builder)
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
