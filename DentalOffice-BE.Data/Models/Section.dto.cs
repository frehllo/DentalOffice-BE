using DentalOffice_BE.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalOffice_BE.Data;

[Table("sections", Schema = ContextSchemaConstants.main)]
public class SectionDto : BaseTableKey<long> 
{
    public long? SectionId { get; set; }
    public string Title { get; set; } = null!;
    public string Route { get; set; } = null!;
    public string? ApiString { get; set; }
    public SectionConfiguration? Configuration { get; set; }
    public SectionDto? Section { get; set; }
    public ICollection<SectionDto>? SubSections { get; set; }
}

public class SectionDtoConfiguration : IEntityTypeConfiguration<SectionDto>
{
    public void Configure(EntityTypeBuilder<SectionDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Route)
            .IsUnique();
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Title)
            .HasColumnName("title");
        builder.Property(e => e.SectionId)
            .HasColumnName("section_id");
        builder.Property(e => e.Route)
            .HasColumnName("route");
        builder.Property(e => e.ApiString)
            .HasColumnName("api_string");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");

        builder
            .HasOne(e => e.Section)
            .WithMany(e => e.SubSections)
            .HasForeignKey(e => e.SectionId);

        builder
            .OwnsOne(e => e.Configuration, d =>
            {
                d.ToJson();
            });
    }
}

