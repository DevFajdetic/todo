import { useState } from "react";
import { Button, Form, FormGroup, InputGroup, Label } from "reactstrap"

export default function AddTodoForm({ onAdded }) {
    const [text, setText] = useState("");
    const [dueDate, setDueDate] = useState(null);
    const {
        run: addTodo
    } = useRequest (
        (text, dueDate) => fetch("api/TodoItem", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ text: text, duedate: dueDate?.toISOString() })
        }),
        {
            manual: true,
            onSuccess: () => {
                setText("");
                setDueDate(null);
                onAdded()
            }
        }
    )

    function handleTextChange(e) {
        setText(e.target.value)
    }

    function handleDateChange(e) {
        setDueDate(e.target.valueAsDate)
    }

    function handleSubmit(e) {
        e.preventDefault();
        addTodo(text, dueDate)
    }

    const dueDateString = dueDate?.toISOString().split("T")[0] ?? ""

    return (
        <Form onSubmit={handleSubmit}> 
            <InputGroup>
                <FormGroup floating>
                    <Input Id="todoText" value={text} onChange={handleTextChange}/>
                    <Label for="todoText">Text</Label>
                </FormGroup>
                <FormGroup floating>
                    <Input Id="todoDueDate" type="date" value={dueDateString} onChange={handleDateChange}/>
                    <Label for="todoDueDate">Due date</Label>
                </FormGroup>

                <Button type="submit">Add todo</Button>
            </InputGroup>
        </Form>
    )
}