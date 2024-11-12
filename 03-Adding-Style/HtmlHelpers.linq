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

#load ".\Boostrap-Library.cs"

void Main()
{
	SetupBootstrap();

	this.Artists.Dump();
	
	H.Tabs(new
	{
		PersonalInfo = H.Grid(new
		{
			FirstName = "Bob",
			LastName = "Jones",
		}),

		Artists = H.Table(this.Artists.Take(20)),
		Tracks = H.Table(this.Tracks.Take(20)),
		Code = Util.SyntaxColorText("var x = 5;", SyntaxLanguageStyle.CSharp)
	}).Dump();

	//H.Tabs(new
	//{
	//	Tables = H.Tabs(this),
	//	NumberList = Enumerable.Range(1, 5),
	//	Table = H.Table(from n in Enumerable.Range(1, 5)
	//					select new { n, sqr = n * n, }),
	//	
	//}).Dump();
}

public static object ToDump(object o)
{

	if (o is string)
	{
		return o;
	}

	//if(o is IEnumerable e) {
	//	//return e.GetType().GetInterfaces();
	//	var records = e.TryGetEnumerableOfTObj();
	//	if(records != null) {
	//		return H.Table(records);
	//	}
	//	
	//	//return "Data";
	//}
	return o;
}

public static void SetupBootstrap()
{

	Reset2();
	Util.HtmlHead.AddCssLink("https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css");
	Util.HtmlHead.AddScriptFromUri("https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js");

}

public static void Reset1()
{
	Util.HtmlHead.AddStyles("""
	/* Reset styles for the following tags */
	h1,
	h2,
	h3,
	h4,
	h5,
	h6 {
	    margin: initial;
	    padding: 0;
	    border: 0;
	    vertical-align: baseline;
	}




	pre,
	code,
	.fixedfont {
	    font-family: initial;
	    font-size: initial;
	}

	a,
	a:visited {
	    text-decoration: initial;
	    font-family: initial;
	    font-weight: initial;
	    cursor: initial;
	}

	a:hover,
	a:visited:hover {
	    text-decoration: initial;
	}

	#final {
	    margin: 0.5em;
		font-size: 82%;

	
	    table {
	        border-collapse: collapse;
	        border-spacing: 0;
	        border: 2px solid #3887B5;
	        margin: initial;
	    }


	    td,
	    th {
	        vertical-align: top;
	        border: 1px solid #3887B5;
	        margin: initial;
	    }

	    th {
	        position: -webkit-sticky;
	        position: sticky;
	        top: 0;
	        z-index: 2
	    }

	    th[scope=row] {
	        position: -webkit-sticky;
	        position: sticky;
	        left: 0;
	        z-index: 2
	    }

	    th {
	        padding: 0.05em 0.3em 0.15em 0.3em;
	        border: 1px solid rgb(50, 50, 50);
	        color: initial;
		    text-align: left;
		    background-color: #ddd;
		    border: 1px solid #777;
		    font-size: .95em;
		    font-family: Segoe UI Semibold, sans-serif;
		    font-weight: bold
	    }
	}

	a,
	a:link {
	    color: initial;
	}

	a:visited {
	    color: initial;
	}


	ol,
	ul {
	    margin: initial;
	    padding-left: initial
	}

	li {
	    margin: initial
	}

	::-ms-clear {
	    display: none
	}

	input,
	textarea,
	button,
	select {
	    font-family: initial;
	    font-size: initial;
	    padding: initial;
	}

	button {
	    padding: initial;
	}

	input,
	textarea,
	select {
	    margin: initial;
	}

	input[type="checkbox"],
	input[type="radio"] {
	    margin: initial;
	    height: initial;
	    width: initial;
	}

	input[type="radio"]:focus,
	input[type="checkbox"]:focus {
	    outline: initial;
	}

	fieldset {
	    margin: initial;
	    border: initial;
	    padding: initial;
	}

	legend {
	    padding: initial;
	}

	input,
	textarea,
	select,
	legend {
	    background: initial;
	    color: initial;
	}

	input,
	textarea,
	select {
	    border: initial;
	}

	input[type="range"] {
	    border: initial;
	}
	""");
}

public static void Reset2()
{
	Util.HtmlHead.AddStyles("""
	
	html,
	body,
	div,
	span,
	iframe,
	p,
	pre,
	a,
	abbr,
	acronym,
	code,
	del,
	em,
	img,
	ins,
	q,
	var,
	i,
	fieldset,
	form,
	label,
	legend,
	table,
	caption,
	tbody,
	tfoot,
	thead,
	tr,
	th,
	td,
	article,
	aside,
	canvas,
	details,
	figure,
	figcaption,
	footer,
	header,
	hgroup,
	nav,
	output,
	section,
	summary,
	time,
	mark,
	audio,
	video {
	    margin: initial;
	    padding: initial;
	    border: initial;
	    vertical-align: initial;
	    font: initial;
	    font-size: initial
	}	
	
	/* Reset styles for the following tags */
	h1,
	h2,
	h3,
	h4,
	h5,
	h6 {
	    margin: initial;
	    padding: initial;
	    border: initial;
	    vertical-align: initial;
	}




	pre,
	code,
	.fixedfont {
	    font-family: initial;
	    font-size: initial;
	}

	a,
	a:visited {
	    text-decoration: initial;
	    font-family: initial;
	    font-weight: initial;
	    cursor: initial;
	}

	a:hover,
	a:visited:hover {
	    text-decoration: initial;
	}

	#final {
	    margin: 0.5em;
		font-size: 82%;
	}

	
	table {
	    border-collapse: initial;
	    border-spacing: initial;
	    border: initial;
		border-width: initial;
	    margin: initial;
	}


	td,
	th {
	    vertical-align: initial;
	    border: initial;
		border-width: initial;
	    margin: initial;
	}

	th {
	    position: -webkit-sticky;
	    position: sticky;
	    top: initial;
	    z-index: initial
	}

	th[scope=row] {
	    position: -webkit-sticky;
	    position: sticky;
	    left: 0;
	    z-index: initial
	}

	th {
	    padding: initial;
	    border: initial;
	    color: initial;
	    text-align: initial;
	    background-color: initial;
	    border: initial;
		border-width: 1px;
	    font-size: initial;
	    font-family: initial;
	    font-weight: initial
	}

	a,
	a:link {
	    color: initial;
	}

	a:visited {
	    color: initial;
	}


	ol,
	ul {
	    margin: initial;
	    padding-left: initial
	}

	li {
	    margin: initial
	}

	::-ms-clear {
	    display: none
	}

	input,
	textarea,
	button,
	select {
	    font-family: initial;
	    font-size: initial;
	    padding: initial;
	}

	button {
	    padding: initial;
	}

	input,
	textarea,
	select {
	    margin: initial;
	}

	input[type="checkbox"],
	input[type="radio"] {
	    margin: initial;
	    height: initial;
	    width: initial;
	}

	input[type="radio"]:focus,
	input[type="checkbox"]:focus {
	    outline: initial;
	}

	fieldset {
	    margin: initial;
	    border: initial;
	    padding: initial;
	}

	legend {
	    padding: initial;
	}

	input,
	textarea,
	select,
	legend {
	    background: initial;
	    color: initial;
	}

	input,
	textarea,
	select {
	    border: initial;
	}

	input[type="range"] {
	    border: initial;
	}
	""");
}