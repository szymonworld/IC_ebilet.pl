using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.ViewModel
{
    public class Category
    {
        public Dictionary<string, List<string>> Categorys { get; set; } = new Dictionary<string, List<string>>();
        public Category()
        {
            Categorys.Add("muzyka", new List<string> { "festiwale", "rock", "hard-heavy", "pop", "piosenka", "jazz-blues", "elektro-techno", "disco-polo", "hip-hop-rap", "ethno", "elektro-techno", "pozostale" });
            Categorys.Add("teatr", new List<string> { "komedia", "tragedia-dramat", "postdramat", "kryminal-thriller", "musicale", "spektakle-muzyczne", "teatr-tanca" });
            Categorys.Add("sport", new List<string> { "sporty-walki", "sporty-druzynowe", "extreme", "pozostale" });
            Categorys.Add("rodzina", new List<string> { "spektakle-dla-dzieci", "warsztaty-edukacja" });
            Categorys.Add("klasyka", new List<string> { "opera-operetka", "balet-taniec", "koncert" });
            Categorys.Add("widowiska", new List<string> { "kabarety", "rewie-show", "inne" });
            Categorys.Add("biznes", new List<string> { "targi" });
        }
    }
}