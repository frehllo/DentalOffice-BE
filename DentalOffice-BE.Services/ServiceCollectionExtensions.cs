using Microsoft.Extensions.DependencyInjection;
using DentalOffice_BE.Data;

namespace DentalOffice_BE.Services;

public static class ServiceCollectionExtensions
{
    public static void InizializeData(this IServiceCollection services)
    {
        services.InizializeDatabase();
    }
}