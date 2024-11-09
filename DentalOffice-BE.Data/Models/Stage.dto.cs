using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DentalOffice_BE.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalOffice_BE.Data;

[Table("stages", Schema = ContextSchemaConstants.main)]
public class StageDto : BaseTableKey<long>
{
    public string Name { get; set; } = null!;
    public ICollection<ProcessDto>? Processes { get; set; }
}

public class StageDtoConfiguration : IEntityTypeConfiguration<StageDto>
{
    public void Configure(EntityTypeBuilder<StageDto> builder)
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

