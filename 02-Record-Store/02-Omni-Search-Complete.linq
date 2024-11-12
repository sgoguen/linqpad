<Query Kind="Program">
  <Connection>
    <ID>54bf9502-9daf-4093-88e8-7177c12aaaaa</ID>
    <NamingService>2</NamingService>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AttachFileName>&lt;ApplicationData&gt;\LINQPad\ChinookDemoDb.sqlite</AttachFileName>
    <DisplayName>Demo database (SQLite)</DisplayName>
    <DriverData>
      <PreserveNumeric1>true</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.Sqlite</EFProvider>
      <MapSQLiteDateTimes>true</MapSQLiteDateTimes>
      <MapSQLiteBooleans>true</MapSQLiteBooleans>
    </DriverData>
  </Connection>
  <Namespace>LINQPad.Controls</Namespace>
</Query>


DumpContainer searchResults = new DumpContainer();

HashSet<Genre> SelectedGenres = new();

void Main()
{
	TextBox searchText = new TextBox(onTextInput: (txt) => this.Search(txt.Text));
	searchText.Dump();
	searchResults.Dump();
	Search("");
}

void Search(string text)
{
	var foundGenres = Genres
		//.Where(a => a.Name.ToLower().Contains(text.ToLower()))
					//.Take(10)
					.ToArray();

	var genres = Util.HorizontalRun(true,
							from g in foundGenres
							select new Button(g.Name, (b) =>
							{
								SelectedGenres.Add(g);
								Search(text);
							})
						 );

	var selectedGenres = Util.HorizontalRun(true,
							from g in SelectedGenres
							select new Button(g.Name, (b) =>
							{
								SelectedGenres.Remove(g);
								Search(text);
							})
						 );

	var noSelectedGenres = SelectedGenres.Count == 0;

	var albums = (from a in Albums
				  let matchingGenres = a.Tracks.Select(t => t.Genre)
				  				  .Where(g => noSelectedGenres || SelectedGenres.Contains(g))
				  //  Filter by selected genres if they were set
				  where matchingGenres.Any()
				  
				  where a.Title.ToLower().Contains(text.ToLower())
				  select new { a.Title })
					.Take(10)
					;

	var artists = Artists.Where(a => a.Name.ToLower().Contains(text.ToLower()))
					.Take(10);

	
	var tracks = Tracks
					// Filter by selected genres if they were set
					.Where(t => SelectedGenres.Contains(t.Genre) || noSelectedGenres)
					.Where(a => a.Name.ToLower().Contains(text.ToLower()))
					.Take(10);

	searchResults.UpdateContent(new
	{
		SelectedGenres = selectedGenres,
		genres,
		albums,
		artists,
		tracks
	});
}
