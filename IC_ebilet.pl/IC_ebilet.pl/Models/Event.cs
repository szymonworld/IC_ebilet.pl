using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.Models
{
    public enum VCategory
    {
        Muzyka,
        Teatr,
        Sport,
        Rodzina,
        Klasyka,
        Widowiska,
        Biznes
    }
    public enum VSubCategory
    {
        Festiwale,
        Rock,
        HardHeavy,
        Pop,
        Piosenka,
        JazzBlues,
        ElektroTechno,
        DiscoPolo,
        HipHopRap,
        Reggae,
        Ethno,
        MuzykaPozostałe,

        Komedia,
        TragediaDramat,
        Postdramat,
        KryminalThriller,
        Musicale,
        SpektakleMuzyczne,
        TeatrTanca,

        SportyWalki,
        SportyMotorowe,
        SportyDruzynowe,
        Extreme,
        SportPozostale,

        SpektakleDlaDzieci,
        RodzinaPozostale,

        OperaOperetka,
        BaletTaniec,
        Koncert,

        Kabarety,
        RewieShow,
        WidowiskaInne,

        Targi
    }
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string BannerLink { get; set; }
        public string Date { get; set; }
        public string Price { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public bool State { get; set; }
    }
}