using Microsoft.EntityFrameworkCore;
using todo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace todo.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<TodoItem> TodoItems { get; set; }
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {

    }
}
