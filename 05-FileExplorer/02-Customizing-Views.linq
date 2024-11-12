<Query Kind="Program" />

void Main()
{
	var thisFile = Util.CurrentQueryPath;
	//thisFile.Dump();
	
	var thisFileInfo = new FileInfo(thisFile);
	//thisFileInfo.Dump();
	
	var thisDirectory = thisFileInfo.Directory;
	thisDirectory.Parent.Dump();
	
}

static object ToDump(object o) {
	
	//if(o is DirectoryInfo d) {
	//	return new {
	//		d.FullName,
	//		//Parent = d.Parent,
	//		//Children = d.EnumerateDirectories()
	//		Children = Util.OnDemand("Child Directories", () => d.EnumerateDirectories()),
	//		Files = Util.OnDemand("Files", () => d.EnumerateFiles())
	//		//
	//	};
	//}
	
	//if(o is FileInfo f) {
	//	return new {
	//		f.FullName,
	//		f.Length,
	//		f.LastWriteTime,
	//		//Details = Util.ToExpando(f),
	//		//View = Util.OnDemand("View", () => File.ReadAllText(f.FullName))
	//	};
	//}
	
	//  Most objects will pass through so they can be rendered
	//  by default
	return o;
}