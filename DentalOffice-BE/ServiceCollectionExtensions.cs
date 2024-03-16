using DentalOffice_BE.Data;
using DentalOffice_BE.Services.Interfaces;
using DentalOffice_BE.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace DentalOffice_BE;

public static class ServiceCollectionExtensions
{
    public static void AddServiceData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISectionService, SectionService>();

        services.AddDbContext<DBContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));
    }
}
