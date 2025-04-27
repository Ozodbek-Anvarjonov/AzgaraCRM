using AzgaraCRM.WebUI.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace AzgaraCRM.WebUI.Extensions;

public static class MigrationExtensions
{
    public static void Migration(this IServiceProvider provider)
    {
        //using (var scope = provider.CreateScope())
        //{
        //    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //    //context.Database.EnsureCreated();
        //    //if (!context.Database.GetPendingMigrations().Any())
        //        //context.Database.Migrate();
        //}
    }
}