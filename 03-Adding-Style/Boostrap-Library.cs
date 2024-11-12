//  Bootstrap Components
using System;
using LINQPad.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using LINQPad;

public static class H
{
	public static Control Button(this string text, Action<Button>? onClick = null) => new Button(text, onClick).SetCss("btn btn-primary");
	public static Control Nested<T>(string tag, string css, IEnumerable<T> values) => new Control(tag, values.Select(v => H.Value(v))).SetCss(css);
	
	public static Control Tag<T>(string tag, string css, T value) => 
		new Control(tag, H.Value(value)).SetCss(css);
	
	
	public static Control Tag<T>(string tag, string css, IEnumerable<T> values) => new Control(tag, values.Select(v => H.Value(v))).SetCss(css);
	public static Control InlineBlock(this Control c) => c.Set(c => c.Styles["display"] = "inline-block");
	public static Control Inline(this Control c) => c.Set(c => c.Styles["display"] = "inline").Set(c => c.Tag = "span");
	public static Control Flex(this Control c) => c.Set(c => c.Styles["display"] = "flex");
	public static Control Span(string value) => new Control("span", value);
	public static Control HStack<T>(IEnumerable<T> values) => Nested("div", "hstack", values);
	public static Control VStack<T>(IEnumerable<T> values) => Nested("div", "vstack", values);
	public static Control Ul<T>(IEnumerable<T> values) => Nested("ul", "list-group list-group-bulleted", values.Select(v => H.Li([v])));
	public static Control Ol<T>(IEnumerable<T> values) => Nested("ol", "list-group list-group-numbered", values.Select(v => H.Tag("li", "list-group-item", v)));
	public static Control Li<T>(IEnumerable<T> values) => Nested("li", "list-group-item", values);
	public static Control Div<T>(IEnumerable<T> values) => Nested("div", "", values);

	public static Control Value<T>(this T value)
	{
		if (O.IsSimple(value))
		{
			return H.Span(value?.ToString());
		}

		if (value is Control c)
		{
			return c;
		}
		else
		{
			var container = new DumpContainer(value);
			return new Control(container).InlineBlock();
		}
	}

	public static Control Grid<T>(T value)
	{
		var properties = ReflectionExtensions.GetProperties(value);
		
		return H.Tag("table", "table table-bordered",
			H.Tag("tbody", "",
				from p in properties
				select H.Tag("tr", "", [
					H.Tag("th", "", p.Name),
					H.Tag("td", "", p.Value)])
			));

	}

	public static TabsView Tabs<T>(T value)
	{
		var properties = ReflectionExtensions.GetProperties(value);
		return new TabsView(properties);

	}

	public static Control Table<T>(IEnumerable<T> values)
	{
		var records = values.Select(v => ReflectionExtensions.GetProperties(v, true)).ToArray();
		var propertyNames = records.SelectMany(r => r.Select(p => p.Name)).Distinct().ToArray();
		return H.Table(new TypedRecords(propertyNames, records));
	}

	public static Control Table(TypedRecords records)
	{
		return H.Tag("table", "table table-sm table-bordered",
			[H.Tag("thead", "",
				H.Tag("tr", "",
					from p in records.PropertyNames
					select H.Tag("th", "", p))),
			H.Tag("tbody", "table-group-divider",
				from r in records.Records.Take(1000)
				select H.Tag("tr", "",
					from p in r
					select H.Tag("td", "", p.Value)))]);
	}
}


public class TabsView(PropertyValue[] properties) {
	DumpContainer SelectedTabContent = new DumpContainer(properties.FirstOrDefault()?.Value);
	public object ToDump() {
		return H.Tag("div", "tabs",
			[H.Tag("nav", "nav nav-tabs",
				from p in properties
				select new Button(p.Name, (_) => SelectedTabContent.UpdateContent(p.Value)).SetCss("nav-item nav-link")),
			H.Tag("div", "tab-content", SelectedTabContent.ToControl())]);		
	}	
}

public static class O
{
	public static bool IsSimple(object value)
	{
		if(value == null) return true;
		return value is string || value.GetType().IsPrimitive || value is decimal || value is DateTime || value is DateTimeOffset || value is TimeSpan;
	}
}

public static class HtmlExtensionMethods
{

	public static Control Set(this Control c, Action<Control> setFn)
	{
		setFn(c); return c;
	}

	//  Fluent API for setting the CSS class
	public static Control SetCss(this Control control, string css) => control.Set(c => c.CssClass = String.IsNullOrEmpty(css) ? null : css);
	public static Control SetTag(this Control c, string newTag) => c.Set(c => c.Tag = newTag);
	public static Control AddCss(this Control c, string css) => c.Set(c => c.CssClass = $"{c.CssClass} {css}");
	public static Control SetId(this Control c, string id) => c.Set(c => c.HtmlElement.ID = id);
	public static Control SetText(this Control c, string text) => c.Set(c => c.HtmlElement.InnerText = text);
	public static Control SetDataAttribute(this Control c, string key, string value) => c.Set(c => c.HtmlElement.SetAttribute($"data-{key}", value));
	public static Control SetRole(this Control c, string role) => c.Set(c => c.HtmlElement.SetAttribute("role", role));



}

public record PropertyValue(string Name, object Value);

public record TypedRecords(string[] PropertyNames, IEnumerable<PropertyValue[]> Records);

public static class ReflectionExtensions
{
	public static bool IsSimpleType(this Type type)
	{
		return type.IsPrimitive
			|| type.IsEnum
			|| type == typeof(string)
			|| type == typeof(decimal)
			|| type == typeof(DateTime)
			|| type == typeof(DateTimeOffset)
			|| type == typeof(TimeSpan);
	}

	public static TypedRecords? TryGetEnumerableOfTObj(this IEnumerable o)
	{
		if (o == null) return null;
		var t = o.GetType();
		var enumerableType = t.GetInterfaces().FirstOrDefault(i =>
		{
			var isGenericIEnumerable = i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>);
			if (!isGenericIEnumerable) return false;
			var args = i.GetGenericArguments();
			return args.Length == 1;
		});

		var elementType = enumerableType.GetGenericArguments()[0];
		if (IsSimpleType(elementType))
		{
			return new TypedRecords(["Value"],
						o.OfType<object>().Select(item => new PropertyValue[] { new PropertyValue("Value", item) }));
		}

		var properties = elementType.GetProperties();
		var propertyNames = properties.Select(p => p.Name).ToArray();
		var records = (from item in o.OfType<object>()
					   select properties.Select(p => new PropertyValue(p.Name, p.GetValue(item))).ToArray());
		return new TypedRecords(propertyNames, records);
	}

	public static PropertyValue[] GetProperties(object obj, bool onlyPrimitive = false)
	{
		if (obj == null) return [];
		var type = obj.GetType();
		return (from p in type.GetProperties()
			    //  Only public and simple types
			    where p.PropertyType.IsSimpleType() || !onlyPrimitive
				select new PropertyValue(p.Name, p.GetValue(obj))).ToArray();
	}
}