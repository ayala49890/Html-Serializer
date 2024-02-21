// See https://aka.ms/new-console-template for more information

using practi2;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

string link = "https://learn.malkabruk.co.il/practicode/projects/pract-2/#_5";
var html = await Load(link);
html = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
var htmlLines = new Regex("<(.*?)>").Split(html).Where(x => x.Length > 0).ToArray();


HtmlElement rootElement = CreateChild(htmlLines[1].Split(' ')[0], null, htmlLines[1]);
rootElement.Name = "html";
rootElement.InnerHtml = "";
rootElement.Parent = null;
ParseHtml(rootElement, htmlLines.Skip(2).ToList());

Console.WriteLine("HTML Tree:");
PrintTree(rootElement, "");
Console.WriteLine("the selector:"); 
//var list = rootElement.FindElements(Selector.ConvertToSelector(" h2#_1"));
//var list = rootElement.FindElements(Selector.ConvertToSelector(" div.md-sidebar__inner nav.md-nav--secondary label.md-nav__title span"));
var list = rootElement.FindElements(Selector.ConvertToSelector("li.md-nav__item a.md-nav__link"));

foreach (var l in list)
{
    Console.WriteLine("* " + l);
}
Console.WriteLine();
HtmlElement ParseHtml(HtmlElement rootElement, List<string> htmlLines)
{
    HtmlElement currentElement = rootElement;

    foreach (var line in htmlLines)
    {
        if (line.StartsWith("/html"))
            break;
        if (line.StartsWith("/"))
        {
            currentElement = currentElement.Parent;
            continue;
        }

        string tagName = line.Split(' ')[0];
        if (!HtmlHelper.MyHelper.FullTags.Contains(tagName))
        {
            currentElement.InnerHtml += line;
            continue;
        }

        HtmlElement child = CreateChild(tagName, currentElement, line);
        currentElement.Children.Add(child);

        if (!HtmlHelper.MyHelper.Tags.Contains(tagName) && !line.EndsWith("/"))
            currentElement = child;
    }
    return rootElement;


}

HtmlElement CreateChild(string name, HtmlElement parent, string line)
{
    HtmlElement child = new HtmlElement();
    child.Name = name;
    child.Parent = parent;
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var attr in attributes)
    {
        string attributeName = attr.ToString().Split('=')[0];
        string attributeValue = attr.ToString().Split('=')[1].Replace("\"", "");

        if (attributeName.ToLower() == "class")
            child.Classes.AddRange(attributeValue.Split(' '));
        else if (attributeName.ToLower() == "id")
            child.Id = attributeValue;
        else child.Attributes.Add(attributeName, attributeValue);
    }
    return child;


}





async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

static void PrintTree(HtmlElement element, string indentation)
{
    Console.WriteLine($"{indentation}{element}");
    foreach (var child in element.Children)
        PrintTree(child, indentation + "  ");
}