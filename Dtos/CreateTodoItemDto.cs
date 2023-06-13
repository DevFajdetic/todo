namespace todo.Dtos;

public record CreateTodoItemDto(
    string Text,
    DateTime? DueDate
);