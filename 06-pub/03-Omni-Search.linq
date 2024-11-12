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


var searchResults = new DumpContainer();
var searchBox = new LINQPad.Controls.TextBox("", "30em", (s) => Search(s.Text));

new
{
	Search = searchBox,
	Results = searchResults
}.Dump();

Search("");

void Search(string search)
{
	searchResults.UpdateContent(new
	{
		SearchText = search,
		Titles = (from t in this.Titles
				  where t.Title.Contains(search)
				  let authors = (from a in t.Titleauthors
								 select $"{a.Author.Au_fname} {a.Author.Au_lname}")
				  select new
				  {
					  t.Title,
					  t.Type,
					  Authors = String.Join(" & ", authors),
					  t.Price,
				  }).Take(5),
		Authors = this.Authors.Where(a => a.Au_fname.StartsWith(search) || a.Au_lname.StartsWith(search))
						.Take(5)
	});
}