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

namespace IC_ebilet.pl.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
        private User Calculation(User user)
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
                    item.precents = Math.Round((item.avr * 100) / (maxcat), 1);
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
                    item.precents = Math.Round((item.avr * 100) / (maxsub), 1);
                }
            }
            user.Favourite.FavCategory = categories;
            user.Favourite.SubCategory = subcategories;

            return user;

        }

        public ActionResult Like(int? id)
        {
            using (var db = new SystemContext())
            {
                if ((id != null) && (!string.IsNullOrEmpty((string)(Session["Email"]))))
                {
                    EventViewModel SelectedEvent = Mapper.Map<Event, EventViewModel>(db.Events.Find(id));
                    string UserEmail = (string)(Session["Email"]);
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
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }
        public ActionResult Dislike(int id)
        {
            using (var db = new SystemContext())
            {
                if ((id != null) && (!string.IsNullOrEmpty((string)(Session["Email"]))))
                {
                    EventViewModel SelectedEvent = Mapper.Map<Event, EventViewModel>(db.Events.Find(id));
                    string UserEmail = (string)(Session["Email"]);
                    User CurrentUser = db.Users.Where(n => n.Email == UserEmail).First();
                    var CurrentCategory = CurrentUser.Favourite.FavCategory.Where(n => n.title == SelectedEvent.Category).First();
                    var CurrentSubCategory = CurrentUser.Favourite.SubCategory.Where(n => n.title == SelectedEvent.SubCategory).First();
                    CurrentSubCategory.dislikes++;
                    CurrentCategory.dislikes++;
                    Avr(CurrentCategory, CurrentSubCategory);
                    CurrentUser = Calculation(CurrentUser);
                    if ((CurrentCategory.dislikes >= 10) && (CurrentCategory.avr <= 30)) CurrentCategory.ban = true;
                    if ((CurrentSubCategory.dislikes >= 10) && (CurrentSubCategory.avr <= 30)) CurrentSubCategory.ban = true;
                    db.SaveChanges();
                }
            }
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }
        public void Avr(TCategory c, TCategory s)
        {
            c.avr = Math.Round(((c.likes + 1.9208) / (c.likes + c.dislikes) - 1.96 * Math.Sqrt((c.likes * c.dislikes) / (c.likes + c.dislikes) + 0.9604) / (c.likes + c.dislikes)) / (1 + 3.8416 / (c.likes + c.dislikes)), 4) * 100;
            s.avr = Math.Round(((s.likes + 1.9208) / (s.likes + s.dislikes) - 1.96 * Math.Sqrt((s.likes * s.dislikes) / (s.likes + s.dislikes) + 0.9604) / (s.likes + s.dislikes)) / (1 + 3.8416 / (s.likes + s.dislikes)), 4) * 100;
            // c.avr = ((c.likes + 1.9208) / ( c.likes + c.dislikes)-1.96 * Math.Sqrt((c.likes * c.dislikes)/(c.likes + c.dislikes)+0.9604)/ (c.likes + c.dislikes)) / (1 + 3.8414/ (c.likes + c.dislikes))
            //if ((CurrentCategory.likes != 0) || (CurrentCategory.dislikes != 0)) CurrentCategory.avr = CurrentCategory.likes / CurrentCategory.dislikes / 2;
            //if ((CurrentSubCategory.likes != 0) || (CurrentSubCategory.dislikes != 0)) CurrentSubCategory.avr = (CurrentSubCategory.likes + CurrentSubCategory.dislikes) / 2;
        }



        public async Task<ActionResult> Index()
        {
            if ((User.Identity.IsAuthenticated) && (Session["Email"] != null))
            {


                ///await SaveEventToDB();
                using (var db = new SystemContext())
                {
                    string UserEmail = (string)(Session["Email"]);
                    User SelUser = db.Users.Single(n => n.Email == UserEmail);
                    List<TCategory> UserCategoryRating = SelUser.Favourite.FavCategory;

                    List<string> SelectedCategory = new List<string>();
                    List<int> SelectedCategoryPrecent = new List<int>();
                    Category AllCateogory = new Category();

                    foreach (var item in AllCateogory.Categorys)
                    {
                        SelectedCategory.Add(item.Key);
                        SelectedCategoryPrecent.Add((int)Math.Round(UserCategoryRating.Where(n => n.title == item.Key).Select(n => n.precents).First(), 0) / 10);
                    }
                    Random rnd = new Random();
                    List<Event> SelectedEvent = new List<Event>();
                    int countPrecent = 0;
                    foreach (var KURWA in SelectedCategory)
                    {
                        string category = KURWA;
                        for (int i = 0; i < SelectedCategoryPrecent[countPrecent]; i++)
                        {
                            int numId = rnd.Next(1, db.Events.Where(n => n.Category == category).Count());
                            
                            SelectedEvent.Add(db.Events.Where(n=>n.Category == category).First());
                           // SelectedEvent.Add(db.Events.Where(n => n.Category == category).Single(m => m.Id == numId));
                        }
                        countPrecent++;
                        ViewBag.WszystkieKategorie = SelectedEvent;

                    }
                }
            }
                
                    

                //Mapper.Map<Event, EventViewModel>(



                //List<EventViewModel> SliderEvents = new List<EventViewModel>();
                //List<EventViewModel> SliderEventsActive = new List<EventViewModel>();
                //Random rnd = new Random();
                //for (int i = 0; i < 6; i++)
                //{
                //    int numId = rnd.Next(1, db.Events.Count());
                //    SliderEvents.Add(Mapper.Map<Event, EventViewModel>(db.Events.Where(n => n.Id == numId).First()));
                //}
                //for (int i = 0; i < 1; i++)
                //{
                //    int numId = rnd.Next(1, db.Events.Count());
                //    SliderEventsActive.Add(Mapper.Map < Event, EventViewModel> (db.Events.Where(n => n.Id == numId).First()));
                //}
                //ViewBag.SliderEvents = SliderEvents;
                //ViewBag.SliderEventsActive = SliderEventsActive;
                using (var db = new SystemContext())
                {
                    List<EventViewModel> events = Mapper.Map<List<Event>, List<EventViewModel>>(db.Events.Where(n => n.Category == "Muzyka").ToList());
                    ViewBag.Muzyka = events;
                }
            return View();
        }
        public async Task SaveEventToDB()
        {
            using (var db = new SystemContext())
            {
                foreach (var item in await GetEvents())
                {
                    foreach (var item2 in item)
                    {
                        if (!db.Events.Any(n => n.Title == item2.Title))
                        {
                            db.Events.Add(Mapper.Map<EventViewModel, Event>(item2));
                        }
                        else
                        {
                            db.Events.Where(n => n.Title == item2.Title).ToList().ForEach(n => n.State = false);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public async Task<List<List<EventViewModel>>> GetEvents()
        {
            List<List<EventViewModel>> AllEvents = new List<List<EventViewModel>>();
            Category cat = new Category();
            Parser parser = new Models.Parser();
            foreach (var item in cat.Categorys)
            {
                foreach (var item2 in item.Value)
                {
                    AllEvents.Add(await parser.DoParse(item.Key, item2));
                }
            }
            return AllEvents;
        }

        public ActionResult Offerts()
        {
            using (var db = new SystemContext())
            {
                List<EventViewModel> events = Mapper.Map<List<Event>, List<EventViewModel>>(db.Events.ToList());
                ViewBag.test2 = events;
            }
            return View();
        }

        public ActionResult Test()
        {
            using (var db = new SystemContext())
            {
                List<EventViewModel> RandomTest = new List<EventViewModel>();
                Random rnd = new Random();
                for (int i = 0; i < 3; i++)
                {
                    int numId = rnd.Next(1, db.Events.Count());
                    RandomTest.Add(Mapper.Map<Event, EventViewModel>(db.Events.Where(n => n.Id == numId).First()));
                }
                ViewBag.RandomTest = RandomTest;
            }
            return View();
        }
    }
}