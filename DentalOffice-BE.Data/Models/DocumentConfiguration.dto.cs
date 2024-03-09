using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using DentalOffice_BE.Common;

namespace DentalOffice_BE.Data;

[Table("document_configurations", Schema = ContextSchemaConstants.main)]
public class DocumentConfigurationDto : BaseTableKey<long>
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public ICollection<DocumentInstanceDto>? Instances { get;}
}

public class DocumentConfigurationDtoConfiguration : IEntityTypeConfiguration<DocumentConfigurationDto>
{
    public void Configure(EntityTypeBuilder<DocumentConfigurationDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name)
            .IsUnique();
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.Name)
            .HasColumnName("name");
        builder.Property(e => e.Content)
            .HasColumnName("content");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");
    }
}
