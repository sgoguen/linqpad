<Query Kind="Program" />

void Main()
{
	var thisFile = Util.CurrentQueryPath.Dump();

	var thisFileInfo = new FileInfo(thisFile);
	//thisFileInfo.Dump();

	var thisDirectory = thisFileInfo.Directory;
	thisDirectory.Parent.Dump();

}

static object ToDump(object o)
{

	//if(o is DirectoryInfo d) {
	//	return new {
	//		d.FullName,
	//		Children = Util.OnDemand("Child Directories", () => d.EnumerateDirectories()),
	//		Files = Util.OnDemand("Files", () => d.EnumerateFiles())
	//	};
	//}	

	if (o is DirectoryInfo d)
	{
		var container = new DumpContainer();
		container.UpdateContent(new
		{
			d.Name,
			Parent = new Hyperlinq(() =>
			{
				container.UpdateContent(d.Parent);
			}, d.Parent.Name),
			Contents = Util.Pivot(new
			{
				Children = d.EnumerateDirectories()
						.Select(c => new Hyperlinq(() =>
						{
							container.UpdateContent(c);
						}, c.Name)),
				Files = d.EnumerateFiles()
			})
		});
		return container;
	}

//	if (o is DirectoryInfo d)
//	{
//		var container = new DumpContainer();
//		container.UpdateContent(new
//		{
//			d.FullName,
//			Parent = new Hyperlinq(() => container.UpdateContent(d.Parent), d.Parent.Name),
//			Content = Util.Pivot(new
//			{
//				Directories = d.EnumerateDirectories()
//								.Select(c => new {
//									Name = new Hyperlinq(() => container.UpdateContent(c), c.Name)
//								}),
//				Files = d.EnumerateFiles()
//			})
//
//
//		});
//		return container;
//	}

	if (o is FileInfo f)
	{
		return new
		{
			f.Name,
			f.Length,
			f.LastWriteTime,
			//Details = Util.ToExpando(f)
		};
	}

	//  Most objects will pass through so they can be rendered
	//  by default
	return o;
}