import AddTodoForm from "./AddTodoForm";
import TodoList from "./TodoList";
import { useRequest } from "ahooks"

export default function Home() {

  const {
    data: todos,
    loading: todosLoading,
    run: getTodos
  } = useRequest(
    () => fetch("api/TodoItem").then(response => response.json()),
  )

  return (
    <div>
      <h1>Todos</h1>
      <AddTodoForm onAdded={} />
      <TodoList todos={todos} />
    </div>
  )
}
