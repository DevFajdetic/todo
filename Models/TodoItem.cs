using System.ComponentModel.DataAnnotations.Schema;

namespace todo.Models;

public class TodoItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    
    public string UserId { get; set; }
    public ApplicationUser? User { get; set; }
}