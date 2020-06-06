using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsCSGO
{
    class Parser
    {
        string url;

        public List<Repoz> db = new List<Repoz>();


        void LUrl()
        {
            url = File.ReadAllText("Url.json");
        }

        public async Task<string> GetHtmlPage()
        {
            LUrl();
            string source = null;
            HttpClient client = new HttpClient();
            var respose = await client.GetAsync(url);
            if (respose != null && respose.StatusCode == System.Net.HttpStatusCode.OK)
            {
                source = await respose.Content.ReadAsStringAsync();
            }
            return source;
        }

        public async Task<IHtmlDocument> Parse()
        {

            var source = await GetHtmlPage();//Получаем HTML код
            var domParser = new HtmlParser();
            var doc = await domParser.ParseDocumentAsync(source);

            var list = new List<string>();

            var items = doc.QuerySelectorAll(".message-main");
            foreach (var item in items)
            {
                list.Add(item.OuterHtml.Substring(10));
            }
            
            string x = @"(([A-Z]|\d){7,8}(<br>|</div))";        
            Regex regex = new Regex(x);

            //MatchCollection matches = regex.Matches(list[0]);

            //List<string> myMatches = new List<string>();

            //foreach (var match in matches)
            //{
            //    myMatches.Add(match.ToString().Split('<')[0]);
            //}

            MatchCollection matches;


            foreach (var mes in list)
            {
                matches = regex.Matches(mes);

                foreach (var match in matches)
                {
                    db.Add(new Repoz(match.ToString().Split('<')[0], System.DateTime.Now));
                }
            }

            int a = 0;



            return doc;
        }
    }
}