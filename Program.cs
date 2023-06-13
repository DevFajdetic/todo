using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using todo.Data;
using todo.Dtos;
using todo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

// GetAll
app.MapGet("api/TodoItem", async (ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal) => {
    var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    var todoItems = await dbContext.TodoItems
    .Where(x => x.UserId == userId)
    .OrderBy(x => x.IsCompleted)
    .ThenBy(x => x.DueDate == null)
    .ThenBy(x => x.DueDate)
    .ThenByDescending(x => x.DateCreated)
    .ToListAsync();

    var todoItemDtos = todoItems.Select(x => new TodoItemDto(x.Id, x.Text, x.DateCreated, x.DueDate, x.IsCompleted));
    return Results.Ok(todoItemDtos);
}).RequireAuthorization();

// Post
app.MapPost("api/TodoItem", async (ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal, CreateTodoItemDto ctid) => {
    var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
    var todoItem = new TodoItem {
        UserId = userId,
        Text = ctid.Text,
        DueDate = ctid.DueDate,
        DateCreated = DateTime.UtcNow,
        IsCompleted = false
    };

    dbContext.Add(todoItem);
    await dbContext.SaveChangesAsync();

    return Results.Ok();
}).RequireAuthorization();

// Update
app.MapPut("api/TodoItem/{id}/ToggleIsCompleted", async (
            ApplicationDbContext dbContext, 
            ClaimsPrincipal claimsPrincipal,
            int id) =>
        {
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            var todoItem = await dbContext
                .TodoItems
                .FindAsync(id);

            if (todoItem is null)
            {
                return Results.NotFound();
            }

            if (todoItem.UserId != userId)
            {
                return Results.Forbid();
            }

            todoItem.IsCompleted = !todoItem.IsCompleted;
            
            await dbContext.SaveChangesAsync();

            return Results.Ok();
}).RequireAuthorization();

app.MapFallbackToFile("index.html");

app.Run();
