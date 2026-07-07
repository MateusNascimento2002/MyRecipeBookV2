using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Infrastructure.DataAccess.DatabaseModels;

namespace MyRecipeBook.Infrastructure.DataAccess;

internal class MyRecipeBookDbContext : DbContext
{
    public MyRecipeBookDbContext(DbContextOptions<MyRecipeBookDbContext> options) : base(options) { }
    
    public DbSet<DatabaseUser> Users { get; set; }
}