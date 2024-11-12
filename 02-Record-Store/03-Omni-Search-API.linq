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
  <NuGetReference>Swashbuckle.AspNetCore</NuGetReference>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.AspNetCore.Mvc</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>




public record SearchRequest(string text, long[] selectGenreIds);

void Main()
{
	var builder = WebApplication.CreateBuilder();

	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	var app = builder.Build();

	app.MapPost("/search", ([FromBody] SearchRequest search) =>
	{
		var text = search.text;
		var selectGenreIds = search.selectGenreIds;
		
		var foundGenres = Genres
						.ToArray();

		var genres = from g in foundGenres
					 select new { g.GenreId, g.Name };

		var selectedGenres = from g in Genres
							 where selectGenreIds.Contains(g.GenreId)
							 select g;

		var noSelectedGenres = !selectedGenres.Any();

		var albums = (from a in Albums
					  let matchingGenres = a.Tracks.Select(t => t.Genre)
											.Where(g => noSelectedGenres || selectedGenres.Contains(g))
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
						.Where(t => selectedGenres.Contains(t.Genre) || noSelectedGenres)
						.Where(a => a.Name.ToLower().Contains(text.ToLower()))
						.Take(10)
						.Select(t => new { t.TrackId, t.Name, t.Composer });

		return new
		{
			//SelectedGenres = selectedGenres,
			genres,
			albums,
			//artists,
			//tracks
		};

	});

	// Test API
	string uriBase = "http://localhost:5000/swagger/";
	StartWebBrowser($"{uriBase}");
	//StartWebBrowser ($"{uriBase}albums/1");

	app.UseSwagger();
	app.UseSwaggerUI();

	app.Run();

	void StartWebBrowser(string uri) =>
		Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });

}

