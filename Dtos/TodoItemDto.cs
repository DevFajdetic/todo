namespace todo.Dtos;

public record TodoItemDto(
    int Id,
    string Text,
    DateTime DateCreated,
    DateTime? DueDate,
    bool isCompleted
);