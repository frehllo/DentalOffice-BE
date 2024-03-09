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

[Table("modules", Schema = ContextSchemaConstants.main)]
public class ModuleDto : BaseTableKey<long>
{
    public string CustomerName { get; set; } = null!;
    public DateTime PrescriptionDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string? Description { get; set; }
    public long StudioId { get; set; }
    public StudioDto? Studio { get; set; }
    public ICollection<ProcessDto>? Processes { get; set; }
    public ICollection<DocumentInstanceDto>? Instances { get; }

}

public class ModuleDtoConfiguration : IEntityTypeConfiguration<ModuleDto>
{
    public void Configure(EntityTypeBuilder<ModuleDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.CustomerName)
            .HasColumnName("customer_name");
        builder.Property(e => e.DeliveryDate)
            .HasColumnName("delivery_date");
        builder.Property(e => e.PrescriptionDate)
            .HasColumnName("prescription_date");
        builder.Property(e => e.StudioId)
            .HasColumnName("studio_id");
        builder.Property(e => e.Description)
            .HasColumnName("description");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");

        builder.HasOne(e => e.Studio)
            .WithMany()
            .HasForeignKey(e => e.StudioId);
    }
}
