<Query Kind="Program" />

void Main()
{
	var thisFile = Util.CurrentQueryPath;
	//thisFile.Dump();
	
	var thisFileInfo = new FileInfo(thisFile);
	thisFileInfo.Dump();
	
}

// You can define other methods, fields, classes and namespaces here
