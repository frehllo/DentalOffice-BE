using DentalOffice_BE.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalOffice_BE.Data;

[Table("processes", Schema = ContextSchemaConstants.main)]
public class ProcessDto : BaseTableKey<long>
{
    [NotMapped]
    public virtual string? Name { get; set; }
    [NotMapped]
    public virtual int[]? Dentals { get; set; }
    public long ModuleId { get; set; }
    public long? SemiProductId { get; set; }
    public long? MetalMaterialId { get; set; }
    public long? MetalLotId { get; set; }
    public long? DentinMaterialId { get; set; }
    public long? DiskMaterialId { get; set; }
    public long? DentinLotId { get; set; }
    public long? EnamelLotId { get; set; }
    public long? DiskLotId { get; set; }
    public long? RiskId { get; set; }
    public long? ColorId { get; set; }
    public IList<long>? StagesIds { get; set; }
    public string? Others { get; set; }
    public string? MetalCustom { get; set; }
    public string? MetalLotCustom { get; set; }
    public string? DentinCustom { get; set; }
    public string? DentinLotCustom { get; set; }
    public string? EnamelCustom { get; set; }
    public string? EnamelLotCustom { get; set; }
    public string? DiskCustom { get; set; }
    public string? DiskLotCustom { get; set; }
    public ICollection<StageDto>? Stages { get; set; }
    public ModuleDto? Module { get; set; }
    public SemiProductDto? SemiProduct { get; set; }
    public MaterialDto? MetalMaterial { get; set; }
    public LotDto? MetalLot { get; set; }
    public MaterialDto? DentinMaterial { get; set; }
    public LotDto? DentinLot { get; set; }
    public LotDto? EnamelLot { get; set;}
    public ColorDto? Color { get; set; }
    public RiskDto? Risk { get; set; }
    public MaterialDto? DiskMaterial { get; set; }
    public LotDto? DiskLot { get; set; }
    public string GetRiepilogo(SemiProductDto? semiProduct, MaterialDto? metal, MaterialDto? dentin, MaterialDto? disk)
    {
        return semiProduct != null ? semiProduct.Name : metal != null ? metal.Name : dentin != null ? dentin.Name : disk != null ? disk.Name : "Processo Generico";
    }
}

public class ProcessDtoConfiguration : IEntityTypeConfiguration<ProcessDto>
{
    public void Configure(EntityTypeBuilder<ProcessDto> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.SemiProductId)
            .HasColumnName("semiproduct_id");
        builder.Property(e => e.MetalMaterialId)
            .HasColumnName("metal_material_id");
        builder.Property(e => e.MetalLotId)
            .HasColumnName("metal_lot_id");
        builder.Property(e => e.DentinMaterialId)
            .HasColumnName("dentin_material_id");
        builder.Property(e => e.DentinLotId)
            .HasColumnName("dentin_lot_id");
        builder.Property(e => e.EnamelLotId)
            .HasColumnName("enamel_lot_id");
        builder.Property(e => e.ColorId)
            .HasColumnName("color_id");
        builder.Property(e => e.RiskId)
            .HasColumnName("risk_id");
        builder.Property(e => e.InsertDate)
            .HasColumnName("insert_date")
            .HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdateDate)
            .HasColumnName("update_date")
            .HasDefaultValueSql("NOW()");
        builder.Ignore(_ => _.Module);

        builder
            .HasOne(e => e.Module)
            .WithMany()
            .HasForeignKey(e => e.ModuleId);

        builder
            .HasOne(e => e.MetalMaterial)
            .WithMany()
            .HasForeignKey (e => e.MetalMaterialId);

        builder
            .HasOne(e => e.MetalLot)
            .WithMany()
            .HasForeignKey(e => e.MetalLotId);

        builder
            .HasOne(e => e.DentinMaterial)
            .WithMany()
            .HasForeignKey(e => e.DentinMaterialId);

        builder
            .HasOne(e => e.DentinLot)
            .WithMany()
            .HasForeignKey(e => e.DentinLotId);

        builder
            .HasOne(e => e.EnamelLot)
            .WithMany()
            .HasForeignKey(e => e.EnamelLotId);

        builder
            .HasOne(e => e.Color)
            .WithMany()
            .HasForeignKey(e => e.ColorId);

        builder
            .HasOne(e => e.Risk)
            .WithMany()
            .HasForeignKey(e => e.RiskId);

        builder
            .HasOne(e => e.DiskMaterial)
            .WithMany()
            .HasForeignKey(e => e.DiskMaterialId);

        builder
            .HasOne(e => e.DiskLot)
            .WithMany()
            .HasForeignKey(e => e.DiskLotId);

        //builder
        //    .HasMany(e => e.Stages)
        //    .WithMany(e => e.Processes)
        //    .UsingEntity(
        //        "processes_stages",
        //        l => l.HasOne(typeof(ProcessDto)).WithMany().HasForeignKey("process_id").HasPrincipalKey(nameof(ProcessDto.Id)),
        //        r => r.HasOne(typeof(StageDto)).WithMany().HasForeignKey("stage_id").HasPrincipalKey(nameof(StageDto.Id)),
        //        j => j.HasKey("stage_id", "process_id"));

        builder
            .HasMany(e => e.Stages)
            .WithMany(e => e.Processes)
            .UsingEntity("processes_stages");
    }
}
