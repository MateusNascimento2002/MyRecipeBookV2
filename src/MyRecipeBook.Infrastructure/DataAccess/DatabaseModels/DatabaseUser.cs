using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess.DatabaseModels;

public class DatabaseUser
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }

    public DatabaseUser(User user)
    {
        Name = user.Name;
        Email = user.Email;
        Password = user.Password;
        IsActive = user.IsActive;
    }
}