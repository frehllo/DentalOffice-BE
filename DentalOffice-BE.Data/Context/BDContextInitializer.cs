using Microsoft.EntityFrameworkCore;

namespace DentalOffice_BE.Data;

internal static class BDContextInitializer
{
    public static void InizializeDatabase(DBContext context)
    {
        Initialize(context);
    }

    private static void Initialize(DBContext context)
    {
        var toBeMigrate = context.Database.GetPendingMigrations().Any();

        if (toBeMigrate)
        {
            context.Database.Migrate();
        }
    }
}
