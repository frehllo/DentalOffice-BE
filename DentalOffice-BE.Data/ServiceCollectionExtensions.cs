using DentalOffice_BE.Data;
using Microsoft.Extensions.DependencyInjection;
using DentalOffice_BE.Common.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DentalOffice_BE.Data;

public static class ServiceCollectionExtensions
{

    public static void AddServiceData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DBContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));
    } 
    public static void InizializeDatabase(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        BDContextInitializer.InizializeDatabase(serviceProvider.GetService<DBContext>().ThrowIfNull());
    }
}
