//  Add these extensions to your My Extensions query to use them in LINQPad.

public static class MyExtensions
{
	public static object Pivot<T>(this T value) {
		return Util.Pivot(value);
	}
	
	public static TextBox ToTextBox(this string text, Action<TextBox> onTextInput) {
		return new TextBox(initialText: text, onTextInput: onTextInput);
	}

	public static Button ToButton(this string text, Action<Button> onClick)
	{
		return new Button(initialText: text, onClick: onClick);
	}
	
	public static DumpContainer ToDumpContainer<T>(this T value) {
		return new DumpContainer(value);
	}

	public static DumpContainer Interact<T, U>(this T model, Func<T, Action<Func<T, T>>, U> render)
	{
		var container = new DumpContainer();
		var view = render(model, UpdateModel);
		container.UpdateContent(view);
		return container;

		void UpdateModel(Func<T, T> update)
		{
			var newModel = update(model);
			var newView = render(newModel, UpdateModel);
			container.UpdateContent(newView);
			model = newModel;
		}
	}
}

public class ObservableContainer<T> {
	DumpContainer container = new();
	private IObservable<T> observable;
	
	public ObservableContainer(IObservable<T> observable)
	{
		this.observable = observable;
		this.observable.Do((v) =>
		{
			container.UpdateContent(v);
		})
		.Subscribe();
	}
	
	public IObservable<T> Source => observable;
	
	
	public object ToDump() {
	
		return container;
	}
}
