using Microsoft.AspNetCore.Identity;

namespace todo.Models;

public class ApplicationUser : IdentityUser
{
    public List<TodoItem> TodoItems { get; set; }
}
