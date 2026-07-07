using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess;

internal class MyRecipeBookDbContext : DbContext
{
    public MyRecipeBookDbContext(DbContextOptions<MyRecipeBookDbContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}