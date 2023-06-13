import { ListGroup, ListGroupItem } from "reactstrap"

export default function TodoList({ todos }) {
    return (
        <div>
            <ListGroup>
                {todos?.map(todo => (
                    <ListGroupItem>
                        <Todo todo={todo}/>
                    </ListGroupItem>
                ))}
            </ListGroup>
        </div>
    )
}