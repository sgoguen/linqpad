<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

//  Define our model
public class TodoList : List<Todo>
{
	//  DumpContainers are updatable controls
	DumpContainer container = new();

	//  Let's make it updatable
	public object ToDump()
	{
		//  Define it here
		TextBox newTextbox = new();

		container.UpdateContent(new
		{
			NewTask = new StackPanel(true,
				newTextbox,
				new Button("Add", (_) => AddText(newTextbox.Text))
			),
			Tasks = from t in this
					select new
					{
						Description = new TextBox(t.Description),
						Completed = new CheckBox("Complete", t.Completed, (_) => MarkComplete(t))
					},
		});

		return container;
	}

	#region "Add text handler"
	void AddText(string v)
	{
		this.Add(new Todo { Description = v, Completed = false });
		container.Refresh();
	}
	#endregion

	#region "Mark Complete"
	void MarkComplete(Todo t)
	{
		this.Remove(t);
		container.Refresh();
	}
	#endregion
}

public class Todo
{
	public required string Description { get; set; }
	public required bool Completed { get; set; }
}

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
	//myTodoList2.Dump();

}

