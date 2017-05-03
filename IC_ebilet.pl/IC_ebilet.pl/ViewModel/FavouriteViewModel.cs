using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.ViewModel
{
    public class FavouriteViewModel
    {
        public List<TCategoryViewModel> FavCategory { get; set; }
        public List<TCategoryViewModel> SubCategory { get; set; }
    }
    public class TCategoryViewModel
    {
        public string title { get; set; }
        public int likes { get; set; } = 0;
        public int dislikes { get; set; }
        public double avr { get; set; }
        public double precents { get; set; } = 0;
        public bool ban { get; set; }
    }
}
