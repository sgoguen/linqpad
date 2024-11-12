<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

//  Define our model
public class TodoList : List<Todo>
{
	//  How do we imagine we might interact with it?
	public object ToDump()
	{
		return new
		{
			NewTask = new StackPanel(true, 
				new TextBox(), 
				new Button("Add")
			),
			Tasks = from t in this
					select new
					{
						Description = new TextBox(t.Description),
						Completed = new CheckBox("Complete", t.Completed)
					},
		};
	}
}

public class Todo
{
	public required string Description { get; set; }
	public required bool Completed { get; set; }
}


public TodoList myTodoList = [
	new Todo { Description = "Show Data", Completed = false },
	new Todo { Description = "Complete UI", Completed = false }
];

void Main()
{
	myTodoList.Dump();

}

