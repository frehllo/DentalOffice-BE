using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalOffice_BE.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalOffice_BE.Data;

[Table("colors", Schema = ContextSchemaConstants.main)]
public class ColorDto : BaseTableKey<long>
{
    public string Code { get; set; } = null!;
    public ICollection<LotDto>? Lots { get; set; }
    public ICollection<ProcessDto>? Processes { get; set; }
}

public class ColorDtoConfiguration : IEntityTypeConfiguration<ColorDto>
{
    public void Configure(EntityTypeBuilder<ColorDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Code)
            .HasColumnName("code");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");
    }
}
