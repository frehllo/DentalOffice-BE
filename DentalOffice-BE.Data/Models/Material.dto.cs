using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DentalOffice_BE.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DentalOffice_BE.Data;

[Table("materials", Schema = ContextSchemaConstants.main)]
public class MaterialDto : BaseTableKey<long>
{
    public string Name { get; set; } = null!;
    public long MaterialTypeId { get; set; }
    public MaterialTypeDto? MaterialType { get; set; }
    [JsonPropertyName("materialProperties")]
    public dynamic? MaterialProperties { get; set; }
}

public class MaterialDtoConfiguration : IEntityTypeConfiguration<MaterialDto>
{
    public void Configure(EntityTypeBuilder<MaterialDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name)
            .IsUnique();
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Name)
            .HasColumnName("name");
        builder.Property(e => e.MaterialTypeId)
            .HasColumnName("material_type_id");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.MaterialProperties)
            .HasColumnType("jsonb");

        builder
            .HasOne(e => e.MaterialType)
            .WithMany(a => a.Materials)
            .HasForeignKey(e => e.MaterialTypeId);
    }
}


