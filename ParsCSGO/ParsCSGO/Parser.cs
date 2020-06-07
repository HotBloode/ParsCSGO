using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParsCSGO
{
    class Parser
    {       
        public ListView mainList;        
        string url;
        string baseUrl = "https://miped.ru/f/threads/promokody-dlja-sajta-gocs-pro-obschaja-tema.67618/page-";
        public List<Repoz> db = new List<Repoz>();

        bool globalFlag = false;
        int lastPage;
        int startPage;      

        void LUrl()
        {           
            startPage = Convert.ToInt32(File.ReadAllText("Url.json"));
        }        
        
        public async void ParsePage()
        {
            while (true)
            {
                LUrl();
                globalFlag = false;
                url = baseUrl + startPage;
                var source = await GetHtmlPage();
                var domParser = new HtmlParser();
                var doc = await domParser.ParseDocumentAsync(source);
                var list = new List<string>();
                lastPage = Convert.ToInt32(doc.QuerySelector("input[max]").Attributes[5].Value);


                for (int i = startPage; i <= lastPage; i++)
                {
                    url = baseUrl + i;
                    bool flag = await ParseCode();
                    if (flag)
                    {
                        globalFlag = true;
                    }
                }

                if (globalFlag)
                {
                    SystemSounds.Beep.Play();                     
                    mainList.Dispatcher.Invoke(new Action(() => mainList.Items.Refresh()));                    
                }                
                //Thread.Sleep(300000);
                File.WriteAllText("Url.json", Convert.ToString(lastPage));
                Thread.Sleep(10000);
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

        async Task<bool> ParseCode()
        {
            bool flagAdd = false;
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
                    if (CheckDublicate(match.ToString().Split('<')[0]))
                    {
                        db.Add(new Repoz(match.ToString().Split('<')[0], System.DateTime.Now));
                        flagAdd = true;
                    }                  
                }                
            }

            return flagAdd;
        }

        bool CheckDublicate(string code)
        {
            for (int i = 0; i < db.Count; i++)
            {
                if(db[i].Code == code)
                {
                    return false;
                }
            }
            return true;
        }
    }
}