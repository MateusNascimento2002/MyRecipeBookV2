using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddPasswordHasher()
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        }

        public void AddRepositories()
        {
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        }

        public void AddUnitOfWork()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public void AddDbContext(IConfiguration configuration)
        {
            services.AddDbContext<MyRecipeBookDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")!);
            });
        }
    }
}