export default function Todo( {todo} ) {
    return (
        <>
        <Input type="checkbox" checked={todo.isCompleted}></Input>
        <span>{todo.Text}</span>
        <span>{todo.dueDate?.toLocaleString()}</span>
        </>
    )
}
