using IC_ebilet.pl.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.Models
{
    public class Favourite
    {
        [Key]
        public int CatId { get; set; }
        public virtual List<TCategory> FavCategory { get; set; }
        public virtual List<TCategory> SubCategory { get; set; }
    }
    public class TCategory
    {
        [Key]
        public int CatId { get; set; }
        public string title { get; set; }
        public int likes { get; set; } = 0;
        public int dislikes { get; set; }
        public double avr { get; set; }
        public double precents { get; set; } = 0;
        public bool ban { get; set; }
    }
}