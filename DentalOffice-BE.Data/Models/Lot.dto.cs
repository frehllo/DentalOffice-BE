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

[Table("lots", Schema = ContextSchemaConstants.main)]
public class LotDto : BaseTableKey<long>
{
    public string Code { get; set; } = null!;
    public long MaterialId { get; set; }
    public long? ColorId { get; set; }
    public ColorDto? Color { get; set; }
    public MaterialDto? Material { get; set; }

}

public class LotDtoConfiguration : IEntityTypeConfiguration<LotDto>
{
    public void Configure(EntityTypeBuilder<LotDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.MaterialId)
            .HasColumnName("material_id");
        builder.Property(e => e.ColorId)
            .HasColumnName("color_id");
        builder.HasOne(e => e.Color)
            .WithMany(e => e.Lots)
            .HasForeignKey(e => e.ColorId);
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");
    }
}
