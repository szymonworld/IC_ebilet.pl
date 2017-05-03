using HtmlAgilityPack;
using IC_ebilet.pl.ViewModel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IC_ebilet.pl.Models
{
    public class Parser
    {
        public async Task<List<EventViewModel>> DoParse(string category, string subcategory)
        {
            List<EventViewModel> events = new List<EventViewModel>();
            List<string> nod = new List<string>();

            HttpClient http = new HttpClient();
            var response = await http.GetByteArrayAsync("https://www.ebilet.pl/" + category + "/" + subcategory);
            string source = Encoding.GetEncoding("UTF-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument resultat = new HtmlDocument();
            resultat.OptionWriteEmptyNodes = true;
            resultat.LoadHtml(source);
            foreach (var item in resultat.DocumentNode.SelectNodes("//div[@id='cubes-wrapper']"))
            {
                foreach (var item2 in item.SelectNodes(".//div[@class='col-sm-4 col-md-3']"))
                {
                    EventViewModel ev = new EventViewModel();
                    ev.Category = category;
                    ev.SubCategory = subcategory;

                    ev.Description = item2.Descendants("div").Where(x => x.Attributes["class"].Value == "desc").ToList().Select(x => x.InnerText).FirstOrDefault();
                    ev.Title = item2.Descendants("div").Where(x => x.Attributes["class"].Value == "overlay-wrapper ").ToList().Where(x => x.Attributes["data-overlay-title"] != null).Select(x => x.Attributes["data-overlay-title"].Value).FirstOrDefault();
                    ev.Date = item2.Descendants("div").Where(x => (x.Attributes["class"].Value == "overlay-wrapper ")).ToList().Where(x => x.Attributes["data-overlay-date"] != null).Select(x => x.Attributes["data-overlay-date"].Value).FirstOrDefault();
                    ev.Location = item2.Descendants("div").Where(x => x.Attributes["class"].Value == "overlay-wrapper ").ToList().Where(x => x.Attributes["data-overlay-location"] != null).Select(x => x.Attributes["data-overlay-location"].Value).FirstOrDefault();
                    ev.Price = item2.Descendants("div").Where(x => x.Attributes["class"].Value == "overlay-wrapper ").ToList().Where(x => x.Attributes["data-overlay-lowest-price"] != null).Select(x => x.Attributes["data-overlay-lowest-price"].Value).FirstOrDefault();
                    ev.Link = "https://www.ebilet.pl" + item2.Descendants("a").Where(x => x.Attributes["class"].Value == "cube").ToList().Where(x => x.Attributes["href"] != null).Select(x => x.Attributes["href"].Value).FirstOrDefault();
                    ev.BannerLink = item2.Descendants("img").Where(x => x.Attributes["src"] != null).Select(x => x.Attributes["src"].Value).FirstOrDefault();
                    ev.State = true;
                    events.Add(ev);
                }
            }
            return events;
        }
    }
}