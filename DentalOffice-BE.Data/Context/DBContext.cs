using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DentalOffice_BE.Data;

public class DBContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DBContext()
    {
        SetOptions();
    }

    public DBContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DBContext(DbContextOptions<DbContext> context) : base(context)
    {
        SetOptions();
    }

    private void SetOptions()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    //Aggiungere db set
    public DbSet<ColorDto> Colors { get; set; }
    public DbSet<CustomerDto> Customers { get; set; }
    public DbSet<DocumentConfigurationDto> DocumentConfigurations { get; set; }
    public DbSet<DocumentInstanceDto> DocumentInstances { get; set; }
    public DbSet<LotDto> Lots { get; set; }
    public DbSet<MaterialDto> Materials { get; set; }
    public DbSet<MaterialTypeDto> MaterialTypes { get; set; }
    public DbSet<ModuleDto> Modules { get; set; }
    public DbSet<ProcessDto> Processes { get; set; }
    public DbSet<RiskDto> Risks { get; set; }
    public DbSet<SectionDto> Sections { get; set; }
    public DbSet<SemiProductDto> SemiProducts { get; set; }
    public DbSet<StageDto> Stages { get; set; }
    public DbSet<StudioDto> Studios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

#pragma warning disable CS0618 // Il tipo o il membro è obsoleto
            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
#pragma warning restore CS0618 // Il tipo o il membro è obsoleto

            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgreSQL"));
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //apply configurations
        modelBuilder.ApplyConfiguration(new ColorDtoConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerDtoConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfigurationDtoConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentInstanceDtoConfiguration());
        modelBuilder.ApplyConfiguration(new LotDtoConfiguration());
        modelBuilder.ApplyConfiguration(new ColorDtoConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialDtoConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialTypeDtoConfiguration());
        modelBuilder.ApplyConfiguration(new ModuleDtoConfiguration());
        modelBuilder.ApplyConfiguration(new ProcessDtoConfiguration());
        modelBuilder.ApplyConfiguration(new RiskDtoConfiguration());
        modelBuilder.ApplyConfiguration(new SectionDtoConfiguration());
        modelBuilder.ApplyConfiguration(new SemiProductDtoConfiguration());
        modelBuilder.ApplyConfiguration(new StageDtoConfiguration());
        modelBuilder.ApplyConfiguration(new StudioDtoConfiguration());
    }

    public override int SaveChanges()
    {
        SetBaseFields();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetBaseFields();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetBaseFields()
    {
        var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseTable && (e.State == EntityState.Added || e.State == EntityState.Modified));
        foreach (var entry in entries)
        {
            ((BaseTable)entry.Entity).UpdateDate = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                ((BaseTable)entry.Entity).InsertDate = DateTime.UtcNow;
            }
            else
            {
                entry.Property(nameof(BaseTable.InsertDate)).IsModified = false;

            }
        }
    }

}
