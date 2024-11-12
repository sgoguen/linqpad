<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

//  Define a minimal model
public class TodoList : List<Todo>
{
}

public class Todo
{
	public required string Description { get; set; }
	public required bool Completed { get; set; }
}

//  Define a basic example 
public TodoList myTodoList = [
	new Todo { Description = "Show Data", Completed = false },
	new Todo { Description = "Complete UI", Completed = false }
];

void Main()
{
	//  Show it
	myTodoList.Dump();
}


