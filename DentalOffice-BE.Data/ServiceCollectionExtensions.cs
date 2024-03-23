using DentalOffice_BE.Data;
using Microsoft.Extensions.DependencyInjection;
using DentalOffice_BE.Common.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DentalOffice_BE.Data;

public static class ServiceCollectionExtensions
{
    public static void InizializeDatabase(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        BDContextInitializer.InizializeDatabase(serviceProvider.GetService<DBContext>().ThrowIfNull());
    }
}
