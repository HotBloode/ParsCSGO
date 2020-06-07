using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
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
        string baseUrl = "https://miped.ru/f/threads/promokody-dlja-sajta-gocs-pro-obschaja-tema.67618/page-";
        public List<Repoz> db = new List<Repoz>();

        int lastPage;
        int startPage;

        void LUrl()
        {           
            startPage = Convert.ToInt32(File.ReadAllText("Url.json"));
        }

        public async Task ParsePageAsync()
        {
            LUrl();
            url = baseUrl + startPage;
            var source = await GetHtmlPage();
            var domParser = new HtmlParser();
            var doc = await domParser.ParseDocumentAsync(source);
            var list = new List<string>();
            lastPage = Convert.ToInt32(doc.QuerySelector("input[max]").Attributes[5].Value);


            for (int i = startPage; i < lastPage; i++)
            {
                url = baseUrl + i;
                //url = baseUrl + startPage;
                ParseCode();
            }

        }

         async Task<string> GetHtmlPage()
        {            
            string source = null;
            HttpClient client = new HttpClient();
            var respose = await client.GetAsync(url);
            if (respose != null && respose.StatusCode == System.Net.HttpStatusCode.OK)
            {
                source = await respose.Content.ReadAsStringAsync();
            }
            return source;
        }

         async Task<IHtmlDocument> ParseCode()
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

            MatchCollection matches;

            foreach (var mes in list)
            {
                matches = regex.Matches(mes);

                foreach (var match in matches)
                {
                    db.Add(new Repoz(match.ToString().Split('<')[0], System.DateTime.Now));
                }
            }

            return doc;
        }
    }
}