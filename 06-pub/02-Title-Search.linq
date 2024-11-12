<Query Kind="Statements">
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


var searchResults = new DumpContainer(this.Titles);
var searchBox = new LINQPad.Controls.TextBox("", "30em", (s) => Search(s.Text));

searchBox.Dump();
searchResults.Dump();

void Search(string search) {
	searchResults.UpdateContent(new {
		SearchText = search,
		Results = this.Titles.Where(t => t.Title.Contains(search)).ToArray()
	});
}