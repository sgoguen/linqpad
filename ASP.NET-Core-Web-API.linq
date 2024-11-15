<Query Kind="Statements">
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
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Mvc</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <IncludeAspNet>true</IncludeAspNet>
</Query>

// The following query demonstrates how to run a web API application using ASP.NET minimal API.
// In this case, we're exposing and endpoint that queries data from a SQLite demo database..

// As with the previous sample, you don't need any NuGet packages when using ASP.NET.
// Just press F4 and tick the ASP.NET checkbox on the bottom-right.

var builder = WebApplication.CreateBuilder();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/albums", () => {
	return TypedResults.Content("<h1>Hey</h1>", "text/html");
	
});

app.MapGet ("/api/albums", (() =>
	 (from a in Albums
	 select new { a.AlbumId, a.Title, Artist = a.Artist.Name }).Dump("Artists")));

app.MapGet ("/api/albums/{id}", ((int id) => Albums
	 .Select (a => new { a.AlbumId, a.ArtistId, a.Title })
	 .FirstOrDefault (a => a.AlbumId == id)));

// Test API
string uriBase = "http://localhost:5000/swagger/";
StartWebBrowser ($"{uriBase}");
//StartWebBrowser ($"{uriBase}albums/1");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

void StartWebBrowser (string uri) =>
	Process.Start (new ProcessStartInfo (uri) { UseShellExecute = true });