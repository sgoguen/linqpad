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
  <Namespace>LINQPad.Controls</Namespace>
</Query>

static DumpContainer searchResults = new DumpContainer();
static UserQuery query;

void Main()
{
	query = this;

	TextBox searchBox = new LINQPad.Controls.TextBox("", "30em", (s) => Search(s.Text));

	new
	{
		Search = Util.HorizontalRun(true, [
			searchBox,
			new Hyperlinq(() => Search(""), "Home")
		]),
		Results = searchResults
	}.Dump();

	Search("");
}

void Search(string search)
{
	searchResults.UpdateContent(new
	{
		Titles = (from t in this.Titles
				  where t.Title.Contains(search)
				  select t),
		Authors = this.Authors.Where(a => a.Au_fname.StartsWith(search) || a.Au_lname.StartsWith(search))
						.Take(5)
	});
}

#region "Views"

static object ToDump(object o)
{
	if (o is decimal d)
	{
		return d.ToString("F2");
	}

	if (o is IEnumerable<Titles> titles)
	{
		return TitlesView(titles);
	}

	if (o is IEnumerable<Authors> authors)
	{
		return AuthorsView(authors);
	}

	return o;
}

static object TitlesView(IEnumerable<Titles> titles)
{
	return (from t in titles.ToArray()
			let authors = (from a in t.Titleauthors
						   let fullName = $"{a.Author.Au_fname} {a.Author.Au_lname}"
						   select new Hyperlinq(() => AuthorPage(a.Author), fullName))
			select new
			{
				Title = new Hyperlinq(() => TitlePage(t), t.Title),
				Type = new Hyperlinq(() => SearchByType(t.Type), t.Type),
				Authors = Util.HorizontalRun(true, authors),
				t.Price,
			});
}

static object AuthorsView(IEnumerable<Authors> authors)
{
	return (from a in authors.ToArray()
			let fullName = $"{a.Au_fname} {a.Au_lname}"
			let titleCount = a.Titleauthors.Count()
			select new
			{
				Name = new Hyperlinq(() => AuthorPage(a), fullName),
				Location = $"{a.City}, {a.State}",
				Titles = new Hyperlinq(() => AuthorPage(a), $"{titleCount} titles"),
			});
}

#endregion



static void SearchByType(string type)
{
	searchResults.UpdateContent(new
	{
		Results = query.Titles.Where(t => t.Type == type)
	});
}

static void AuthorPage(Authors a)
{
	searchResults.UpdateContent(new
	{
		Author = a,
		Titles = query.Titles.Where(t => t.Titleauthors.Any(ta => ta.Au_id == a.Au_id))
	});
}

static void TitlePage(Titles t)
{
	searchResults.UpdateContent(t);
}