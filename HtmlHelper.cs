using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace practi2
{
    internal class HtmlHelper
    {
        private static readonly HtmlHelper _myHelper = new HtmlHelper();
        public static HtmlHelper MyHelper => _myHelper;
        public List<string> FullTags { get; set; }
        public List<string> Tags { get; set; }
        private HtmlHelper()
        {
            string jsonText = File.ReadAllText("files/HtmlTags.json");
            FullTags = JsonSerializer.Deserialize<List<string>>(jsonText);

            jsonText = File.ReadAllText("files/HtmlVoidTags.json");
            Tags = JsonSerializer.Deserialize<List<string>>(jsonText);
        }
    }
}

