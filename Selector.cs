using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace practi2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector ConvertToSelector(string query)
        {
            string[] querys = query.Split(' ');
            Selector root = new Selector();
            Selector current = root;
            foreach (string q in querys)
            {
                string[] selectors = new Regex("(?=[#\\.])").Split(q).Where(s => s.Length > 0).ToArray();
                foreach (string selector in selectors)
                {
                    if (selector.StartsWith("#"))
                        current.Id = selector.Substring(1);
                    else
                        if (selector.StartsWith("."))
                        current.Classes.Add(selector.Substring(1));
                    else
                        if (HtmlHelper.MyHelper.FullTags.Contains(selector))
                        current.TagName = selector;
                    else
                        throw new ArgumentException($"Invalid HTML tag name: {selector}");
                }
                Selector child = new Selector();
                current.Child = child;
                child.Parent = current;
                current = child;
            }
            current.Parent.Child = null;

            return root;
        }
        public override string ToString()
        {
            string str = "";
            if (TagName != null) str += "TagName: " + TagName;
            if (Id != null) str += " Id: " + Id;
            if (Classes.Count > 0)
            {
                str += " classes: ";
                foreach (var c in Classes)
                    str += c + " ";
            }
            return str;
        }
    }
}
