using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;

namespace MyRecipeBook.Infrastructure.Migrations;

public class DatabaseMigration
{
    public static async Task ExecuteMigrations(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<MyRecipeBookDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}