using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Security.PasswordHashing;
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
    }
}