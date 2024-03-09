using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DentalOffice_BE.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalOffice_BE.Data;

[Table("risks", Schema = ContextSchemaConstants.main)]
public class RiskDto : BaseTableKey<long>
{
    public string Description { get; set; } = null!;
    public ICollection<ProcessDto>? Processes { get; set; }
}

public class RiskDtoConfiguration : IEntityTypeConfiguration<RiskDto>
{
    public void Configure(EntityTypeBuilder<RiskDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Description)
            .IsUnique();
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Description)
            .HasColumnName("description");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");
    }
}
