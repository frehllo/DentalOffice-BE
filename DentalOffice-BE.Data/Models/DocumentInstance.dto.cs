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

[Table("document_instances" , Schema = ContextSchemaConstants.main)]
public class DocumentInstanceDto : BaseTableKey<long>
{
    public long ConfigurationId { get; set; }
    public long ModuleId { get; set; }
    public DocumentConfigurationDto? Configuration { get; set; }
    public ModuleDto? Module { get; set; }
}

public class DocumentInstanceDtoConfiguration : IEntityTypeConfiguration<DocumentInstanceDto>
{
    public void Configure(EntityTypeBuilder<DocumentInstanceDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.ConfigurationId)
            .HasColumnName("configuration_id");
        builder.Property(e => e.ModuleId)
            .HasColumnName("module_id");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");

        builder
            .HasOne(e => e.Configuration)
            .WithMany(a => a.Instances)
            .HasForeignKey(e => e.ConfigurationId);

        builder
            .HasOne(e => e.Module)
            .WithMany(a => a.DocumentInstances)
            .HasForeignKey(e => e.ModuleId);
    }
}
