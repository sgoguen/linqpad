<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

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
						}, c.Name)).ToArray(),
				Files = d.EnumerateFiles().ToArray()
			}),
			UsedSpace = Util.OnDemand("Used Disk Space", () => CalculateSpace(d))
		});
		return container;
	}

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

static object CalculateSpace(DirectoryInfo d)
{
	var container = new DumpContainer();


	Task.Run(() =>
	{
		//  Use a queue to recursively calculate disk space
		Queue<DirectoryInfo> queue = new Queue<DirectoryInfo>();
		queue.Enqueue(d);
		long totalSize = 0;
		long fileCount = 0;

		while (queue.Count > 0)
		{
			var currentDir = queue.Dequeue();
			try
			{
				foreach (var file in currentDir.EnumerateFiles())
				{
					totalSize += file.Length;
					fileCount += 1;
					container.UpdateContent(new { totalSize, fileCount, Queued = queue.Count });
					Thread.Sleep(100);
				}
				foreach (var subDir in currentDir.EnumerateDirectories())
				{
					queue.Enqueue(subDir);
				}
			}
			catch (UnauthorizedAccessException)
			{
				// Ignore folders to which we don't have access
			}
			catch (DirectoryNotFoundException)
			{
				// Ignore folders that may have been deleted since the last scan
			}
		}
	});

	
	return container;
}
