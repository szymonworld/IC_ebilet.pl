using AutoMapper;
using HtmlAgilityPack;
using IC_ebilet.pl.Models;
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
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;

namespace IC_ebilet.pl.Helpers
{
    public class LogInOperation : Controller
    {
        public void TasteCalculation(List<Event> events, string UserEmail)
        {
                using (var db = new SystemContext())
                {
                    
                    User SelUser = db.Users.Single(n => n.Email == UserEmail);
                    List<TCategory> FavCategory = SelUser.Favourite.FavCategory;
                    List<TCategory> FavSubCategory = SelUser.Favourite.SubCategory;
                    foreach (var item in events)
                    {
                        double Avgcat = FavCategory.Where(n => n.title == item.Category).Select(m => m.avr).First();
                        double Avgsubcat = FavSubCategory.Where(n => n.title == item.SubCategory).Select(m => m.avr).First();
                        double Avg = (Avgcat + Avgsubcat) / 2;
                        item.TasteOfUser = Math.Round(Avg,0);
                    }
                }
        }
        public User Calculation(User user)
        {
            var categories = user.Favourite.FavCategory;
            var subcategories = user.Favourite.SubCategory;
            double maxcat = 0;
            double maxsub = 0;
            foreach (var item in categories)
            {
                maxcat += item.avr;
            }
            foreach (var item in categories)
            {
                if (item.avr >= 1)
                {
                    item.precents = Math.Round((item.avr * 100) / (maxcat), 2);
                }
            }

            foreach (var item in subcategories)
            {
                maxsub += item.avr;
            }
            foreach (var item in subcategories)
            {
                if (item.avr >= 1)
                {
                    item.precents = Math.Round((item.avr * 100) / (maxsub), 2);
                }
            }
            user.Favourite.FavCategory = categories;
            user.Favourite.SubCategory = subcategories;

            return user;

        }
        public void LikeOperation(int id, string UserEmail)
        {
            using (var db = new SystemContext())
            {
                EventViewModel SelectedEvent = Mapper.Map<Event, EventViewModel>(db.Events.Find(id));
                
                User CurrentUser = db.Users.Where(n => n.Email == UserEmail).First();
                var CurrentCategory = CurrentUser.Favourite.FavCategory.Where(n => n.title == SelectedEvent.Category).First();
                var CurrentSubCategory = CurrentUser.Favourite.SubCategory.Where(n => n.title == SelectedEvent.SubCategory).First();
                CurrentSubCategory.likes++;
                CurrentCategory.likes++;
                Avr(CurrentCategory, CurrentSubCategory);
                CurrentUser = Calculation(CurrentUser);
                db.SaveChanges();
            }
        }
        public void DislikeOperation(int id,string UserEmail)
        {
            using (var db = new SystemContext())
            {
                EventViewModel SelectedEvent = Mapper.Map<Event, EventViewModel>(db.Events.Find(id));
                User CurrentUser = db.Users.Where(n => n.Email == UserEmail).First();
                var CurrentCategory = CurrentUser.Favourite.FavCategory.Where(n => n.title == SelectedEvent.Category).First();
                var CurrentSubCategory = CurrentUser.Favourite.SubCategory.Where(n => n.title == SelectedEvent.SubCategory).First();
                CurrentSubCategory.dislikes++;
                CurrentCategory.dislikes++;
                Avr(CurrentCategory, CurrentSubCategory);
                CurrentUser = Calculation(CurrentUser);
                if ((CurrentCategory.dislikes >= 10) && (CurrentCategory.avr <= 30)) CurrentCategory.ban = true;
                if ((CurrentSubCategory.dislikes >= 10) && (CurrentSubCategory.avr <= 30)) CurrentSubCategory.ban = true;
                //CurrentUser.BannedEventID.Add(id);
                db.SaveChanges();
            }
        }
        public void Avr(TCategory c, TCategory s)
        {
            c.avr = Math.Round(((c.likes + 1.9208) / (c.likes + c.dislikes) - 1.96 * Math.Sqrt((c.likes * c.dislikes) / (c.likes + c.dislikes) + 0.9604) / (c.likes + c.dislikes)) / (1 + 3.8416 / (c.likes + c.dislikes)), 4) * 100;
            s.avr = Math.Round(((s.likes + 1.9208) / (s.likes + s.dislikes) - 1.96 * Math.Sqrt((s.likes * s.dislikes) / (s.likes + s.dislikes) + 0.9604) / (s.likes + s.dislikes)) / (1 + 3.8416 / (s.likes + s.dislikes)), 4) * 100;
        }
    }
}