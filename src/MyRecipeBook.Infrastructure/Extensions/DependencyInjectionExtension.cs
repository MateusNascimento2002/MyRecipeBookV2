using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Interfaces.Repositories.UnitOfWork;
using MyRecipeBook.Domain.Interfaces.Repositories.Users;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Identity;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.Security.Tokens;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration)
        {
            services.AddPasswordHasher();
            services.AddRepositories();
            services.AddUnitOfWork();
            services.AddUtilities();
            services.AddDbContext(configuration);
            services.AddTokenGenerator(configuration);
        }

        private void AddPasswordHasher()
        {
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>();
        }

        private void AddRepositories()
        {
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        }

        private void AddUnitOfWork()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private void AddDbContext(IConfiguration configuration)
        {
            services.AddDbContext<MyRecipeBookDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")!);
            });
        }

        private void AddTokenGenerator(IConfiguration configuration)
        {
            var expirationTimeInMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeInMinutes");
            var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

            services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenHandler(expirationTimeInMinutes, signingKey));
        }

        private void AddUtilities()
        {
            services.AddScoped<ILoggedUser, LoggedUser>();
        }
    }
}