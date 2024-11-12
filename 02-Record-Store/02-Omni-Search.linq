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


DumpContainer container = new();

void Main()
{
	TextBox textbox = new(onTextInput: (t) => {
		Search(t.Text);
	});
	
	textbox.Dump();
	container.Dump();
	
	Search("");
	
}

HashSet<Genre> selectedGenres = new();

void Search(string search) {
	var albums = (from a in this.Albums
				 where a.Title.ToLower().Contains(search.ToLower())
				 select a).Take(10);

	var artists = (from a in this.Artists
				 where a.Name.ToLower().Contains(search.ToLower())
				 select a).Take(10);

	var genres = this.Genres.ToArray().Select(g => new Button(g.Name, (_) =>
	{
		selectedGenres.Add(g);
		container.Refresh();
	}));

	container.UpdateContent(new {
		selectedGenres,
		genres = Util.HorizontalRun(true, genres),
		albums,
		artists
	});
}