using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace practi2
{
    internal class HtmlElement
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                if (this != current)
                    yield return current;

                foreach (HtmlElement child in current.Children)
                    queue.Enqueue(child);
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this.Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();

            foreach (var child in this.Descendants())
                child.FindElementsRec(selector, result);
            return result;
        }
        public void FindElementsRec(Selector selector, HashSet<HtmlElement> res)
        {
            if (!IsMatch(selector))
                return;

            if (selector.Child == null)
                res.Add(this);
            else
                foreach (var child in Descendants())
                    child.FindElementsRec(selector.Child, res);
        }
        private bool IsMatch(Selector selector)
        {
            return ((selector.TagName == null || Name.Equals(selector.TagName))
                && (selector.Id == null || selector.Id.Equals(Id))
                && (selector.Classes.Intersect(Classes).Count() == selector.Classes.Count));
        }
        public override string ToString()
        {
            string str = "";
            if (Name != null) str += "Name: " + Name;
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
