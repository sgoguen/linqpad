<Query Kind="Program">
  <NuGetReference>System.Interactive</NuGetReference>
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	var folder = new DirectoryInfo(@"C:\Projects\github.com");
	var fileSizes = (from f in folder.ToTree(f => f.EnumerateDirectories())
					 where !f.Name.StartsWith(".")
					 where f.Name != "node_modules"
					 select f)
					.MaxDepth(5)

					;

	//Enumerable.Range(1, 10)
	//	.Scan (0, (x, y) => x + y)
	//	.Dump();
	//return;

	fileSizes
		.ScanUp(
			(node) => new
			{
				node.Name,
				fileCount = node.EnumerateFiles().Count(),
				totalBytes = node.EnumerateFiles().Sum(fi => fi.Length)
			},
			(node, childValues) => new
			{
				node.Name,
				fileCount = node.fileCount + childValues.Sum(c => c.fileCount),
				totalBytes = node.totalBytes + childValues.Sum(c => c.totalBytes)
			})
		.CollapsedView()
		.Dump();

}


#region "Tree Library"

public class CollapsedView<T>(ITree<T> tree)
{
	public object ToDump()
	{
		return new { tree.Value, Children = Util.OnDemand("Expand", () => tree.Children.Select(c => new CollapsedView<T>(c))) };
	}
}

public static class Tree
{
	public static ITree<T> ToTree<T>(this T value) => new SingleTree<T>(value);

	public static ITree<T> ToTree<T>(this T value, Func<T, IEnumerable<T>> f) => new DeepTree<T>(value, f);

	public static ITree<T> Where<T>(this ITree<T> tree, Func<T, bool> predicate) => new FilteredTree<T>(tree, predicate);

	public static ITree<U> Select<T, U>(this ITree<T> tree, Func<T, U> selector) => new MappedTree<T, U>(tree, selector);
	public static ITree<T> MaxDepth<T>(this ITree<T> tree, int maxDepth) => new MaxDepthTree<T>(tree, maxDepth);

	public static ITree<U> ScanUp<T, U>(this ITree<T> tree, Func<T, U> init, Func<U, U[], U> fold) => new ScannedUp2<T, U>(tree, init, fold);

	public static U FoldUp<T, U>(this ITree<T> tree, Func<T, U> init, Func<U, U[], U> fold)
	{
		//  Do Depth First Fold up
		var value = init(tree.Value);
		var children = (from c in tree.Children
						select c.FoldUp(init, fold)).ToArray();
		return fold(value, children);
	}


	public static object CollapsedView<T>(this ITree<T> tree) => new CollapsedView<T>(tree);

	public static IEnumerable<T> DepthFirst<T>(this ITree<T> tree)
	{
		yield return tree.Value;
		foreach (var child in tree.Children)
		{
			foreach (var value in DepthFirst(child))
			{
				yield return value;
			}
		}
	}

	public static IEnumerable<T> BreadthFirst<T>(this ITree<T> tree)
	{
		var queue = new Queue<ITree<T>>();
		yield return tree.Value;
		foreach (var child in tree.Children)
		{
			queue.Enqueue(child);
		}
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			yield return current.Value;
			foreach (var child in current.Children)
			{
				queue.Enqueue(child);
			}
		}
	}

}

public interface ITree<T>
{
	T Value { get; }
	IReadOnlyList<ITree<T>> Children { get; }

	public object ToDump()
	{
		return this.Value;
	}
}

public class SingleTree<T>(T value) : ITree<T>
{
	public T Value => value;
	public IReadOnlyList<ITree<T>> Children => [];
}

public class DeepTree<T>(T value, Func<T, IEnumerable<T>> f) : ITree<T>
{
	public T Value => value;

	public IReadOnlyList<ITree<T>> Children => f(value).Select(n => new DeepTree<T>(n, f)).ToList();
}

public class FilteredTree<T>(ITree<T> tree, Func<T, bool> test) : ITree<T>
{
	public T Value => tree.Value;

	public IReadOnlyList<ITree<T>> Children => tree.Children.Where(child => test(child.Value)).Select(child => new FilteredTree<T>(child, test)).ToList();
}

public class MappedTree<T, U>(ITree<T> tree, Func<T, U> selector) : ITree<U>
{
	public U Value => selector(tree.Value);
	public IReadOnlyList<ITree<U>> Children => tree.Children.Select(child => new MappedTree<T, U>(child, selector)).ToList();
}

public class ScannedUp<T, U> : ITree<U>
{
	Lazy<IReadOnlyList<ITree<U>>> children;
	Lazy<U> foldedValue;


	public ScannedUp(ITree<T> tree, Func<T, U[], U> fold)
	{
		children = new(() => tree.Children.Select(child => new ScannedUp<T, U>(child, fold)).ToList());
		foldedValue = new(() =>
		{
			var value = tree.Value;
			var childValues = children.Value.Select(c => c.Value).ToArray();
			return fold(value, childValues);
		});
	}

	public U Value => foldedValue.Value;
	public IReadOnlyList<ITree<U>> Children => children.Value;
}

public class ScannedUp2<T, U> : ITree<U>
{
	Lazy<IReadOnlyList<ITree<U>>> children;
	Lazy<U> foldedValue;


	public ScannedUp2(ITree<T> tree, Func<T, U> select, Func<U, U[], U> fold)
	{
		children = new(() => tree.Children.Select(child => new ScannedUp2<T, U>(child, select, fold)).ToList());
		foldedValue = new(() =>
		{
			var value = select(tree.Value);
			var childValues = children.Value.Select(c => c.Value).ToArray();
			return fold(value, childValues);
		});
	}

	public U Value => foldedValue.Value;
	public IReadOnlyList<ITree<U>> Children => children.Value;
}

public class ScannedDownTree<T, U> : ITree<U>
{
	Lazy<IReadOnlyList<ITree<U>>> children;
	U foldedValue;


	public ScannedDownTree(ITree<T> tree, Func<T, U> select, Func<U, U[], U> fold)
	{
		children = new(() => tree.Children.Select(child => new ScannedDownTree<T, U>(child, select, fold)).ToList());

		var value = select(tree.Value);
		var childValues = tree.Children.Select(c => select(c.Value)).ToArray();
		foldedValue = fold(value, childValues);
	}

	public U Value => foldedValue;
	public IReadOnlyList<ITree<U>> Children => children.Value;
}

public class MaxDepthTree<T>(ITree<T> tree, int maxDepth) : ITree<T>
{
	public T Value => tree.Value;

	public IReadOnlyList<ITree<T>> Children => (maxDepth <= 1) ? [] : tree.Children.Select(child => new MaxDepthTree<T>(child, maxDepth - 1)).ToList();
}



#endregion