using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.ViewModel
{
    public class EventViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Link")]
        public string Link { get; set; }
        [Display(Name = "Baner")]
        public string BannerLink { get; set; }
        [Display(Name = "Data")]
        public string Date { get; set; }
        [Display(Name = "Cena")]
        public string Price { get; set; }
        [Display(Name = "Lokacja")]
        public string Location { get; set; }
        [Display(Name = "Kategoria")]
        public string Category { get; set; }
        [Display(Name = "Podkategoria")]
        public string SubCategory { get; set; }
        public bool State { get; set; }
        public double TasteOfUser { get; set; } 
    }
}