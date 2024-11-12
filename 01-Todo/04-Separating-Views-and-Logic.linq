<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

//  Back to a Simple Model
public class TodoList : List<Todo>
{
	public void AddText(string v)
	{
		this.Add(new Todo { Description = v, Completed = false });
	}

	public void MarkComplete(Todo t)
	{
		this.Remove(t);
	}
}

public class Todo
{
	public required string Description { get; set; }
	public required bool Completed { get; set; }
}

#region "Adding Custom Views to Anything"
//  Define Custom Views for ANY type
public static object ToDump(object o)
{
	if (o is TodoList todoList)
	{
		return ShowTodoList(todoList);
	}

	return o;
}
#endregion

#region "ShowTodoList Custom View"
static object ShowTodoList(TodoList todoList)
{
	DumpContainer container = new();

	TextBox newTextbox = new();

	container.UpdateContent(new
	{
		NewTask = new StackPanel(true,
			newTextbox,
			new Button("Add", (_) =>
			{
				todoList.AddText(newTextbox.Text);
				container.Refresh();
			})
		),
		Tasks = from t in todoList
				select new
				{
					Description = new TextBox(t.Description),
					Completed = new CheckBox("Complete", t.Completed, (_) =>
					{
						todoList.MarkComplete(t);
						container.Refresh();
					})
				},
	});

	return container;
}
#endregion


void Main()
{

	TodoList myTodoList1 = [
		new Todo { Description = "Show Data", Completed = false },
		new Todo { Description = "Complete UI", Completed = false }
	];

	TodoList myTodoList2 = [
		new Todo { Description = "Refactor Code", Completed = false }
	];

	myTodoList1.Dump();
	myTodoList2.Dump();

}

