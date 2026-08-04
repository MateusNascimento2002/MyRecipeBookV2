using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.DataAccess;
using Testcontainers.PostgreSql;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public MyRecipeBookApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder("postgres:18.4")
            .WithDatabase("meulivrodereceitas")
            .Build();
    }

    public UserIdentityManager User1 { get; private set; } = null!;
    public UserIdentityManager UserWithoutRecipes { get; private set; } = null!;
    public string TOKEN_USER_NOT_FOUND_IN_DATABASE { get; private set; } = string.Empty;


    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await SeedDatabase();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return _postgreSqlContainer.StopAsync();
    }

    private async Task SeedDatabase()
    {
        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

        var (user, password) = UserBuilder.Build();
        var recipe = RecipeBuilder.Build(user);

        user.Password = passwordHasher.HashPassword(password);

        var (userWithoutRecipes, userWithoutRecipesPassword) = UserBuilder.Build();

        userWithoutRecipes.Password = passwordHasher.HashPassword(userWithoutRecipesPassword);

        await dbContext.Users.AddAsync(user);
        await dbContext.Users.AddAsync(userWithoutRecipes);
        await dbContext.Recipes.AddAsync(recipe);
        await dbContext.SaveChangesAsync();

        var user1AccessToken = accessTokenGenerator.Generate(user);
        var userWithoutRecipesAccessToken = accessTokenGenerator.Generate(userWithoutRecipes);

        TOKEN_USER_NOT_FOUND_IN_DATABASE = accessTokenGenerator.Generate(new MyRecipeBook.Domain.Entities.User());

        User1 = new UserIdentityManager(user, recipe, password, user1AccessToken);
        UserWithoutRecipes = new UserIdentityManager(userWithoutRecipes, userWithoutRecipesPassword,
            userWithoutRecipesAccessToken);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:PostgreSQL"] = _postgreSqlContainer.GetConnectionString()
                };

                configuration.AddInMemoryCollection(parameters);
            });
    }
}