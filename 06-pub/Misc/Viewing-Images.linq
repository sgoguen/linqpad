<Query Kind="Program">
  <Connection>
    <ID>92a30605-0f47-4b08-83ce-eaeef35659fc</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Database>pubs</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

void Main()
{
	//  CSS Trick to change background
	Util.RawHtml ("<style>tr {background:#fff}</style>").Dump();
	
	//  Show the Images
	(from p in this.Pub_info.ToArray()
	select new {
		Logo = Util.Image(p.Logo)
	}).Dump();
}

// You can define other methods, fields, classes and namespaces here
