using MyRecipeBook.Domain.Interfaces;

namespace MyRecipeBook.Domain.Entities;

public class User : IHaveId
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public bool IsActive { get; set; } = true;

    private User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
    
    public static User Create(string name, string email, string password)
    {
        return new User(name, email, password);
    }
}