using Bogus;
using CommonTestUtilities.Repositories;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string rawPassword) Build()
    {
        var (password, hashedPassword) = GenerateRandomPassword();
        
        var user = new Faker<User>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, _ => hashedPassword);
        
        return (user, password);
    }

    private static (string rawPassowrd, string hashedPassword) GenerateRandomPassword()
    {
        var passwordEncripter = new IPasswordHasherBuilder().Build();
        var randomPassword = new Faker().Internet.Password();
        return (randomPassword, passwordEncripter.HashPassword(randomPassword));
    }
}