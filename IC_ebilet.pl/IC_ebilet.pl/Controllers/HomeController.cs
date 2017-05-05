using AutoMapper;
using HtmlAgilityPack;
using IC_ebilet.pl.Helpers;
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

        public ActionResult Like(int id)
        {
            LogInOperation operation = new LogInOperation();
                if ((id != null) && (!string.IsNullOrEmpty((string)(Session["Email"]))))
                {
                operation.LikeOperation(id, (string)(Session["Email"]));
                }
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }
        public ActionResult Dislike(int id)
        {
            LogInOperation operation = new LogInOperation();
            using (var db = new SystemContext())
            {
                if ((id != null) && (!string.IsNullOrEmpty((string)(Session["Email"]))))
                {
                    operation.DislikeOperation(id, (string)(Session["Email"]));
                }
            }
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
        }
        public async Task<ActionResult> Index()
        {
            //await SaveEventToDB();
            if ((User.Identity.IsAuthenticated) && (Session["Email"] != null))
            {  
                using (var db = new SystemContext())
                {
                    string UserEmail = (string)(Session["Email"]);
                    User SelUser = db.Users.Single(n => n.Email == UserEmail);
                    List<TCategory> UserCategoryRating = SelUser.Favourite.FavCategory;

                    Dictionary<string, int> SelectedCategory = new Dictionary<string, int>();
                    //List<string> SelectedCategory = new List<string>();
                    //List<int> SelectedCategoryPrecent = new List<int>();
                    Category AllCateogory = new Category();

                    foreach (var item in AllCateogory.Categorys)
                    {
                        SelectedCategory.Add(item.Key, (int)UserCategoryRating.Where(n => n.title == item.Key).Select(n => n.precents).First());
                    }
                    Random rnd = new Random();
                    List<Event> SelectedEvent = new List<Event>();
                    int countPrecent = 0;

                    foreach (var item in SelectedCategory)
                    {
                        int numId = 0;
                        for (int i = 0; i < item.Value; i++)
                        {
                            int start = db.Events.Where(n => n.Category == item.Key).Select(n => n.Id).First();
                            int end = start + db.Events.Where(n => n.Category == item.Key).Count() -1;
                            numId = rnd.Next(start, end);
                            List<Event> e = db.Events.Where(n => n.Category == item.Key.ToString()).ToList();
                            SelectedEvent.Add(e.Where(n=>n.Id == numId).FirstOrDefault());
                        }
                        
                    }
                    ViewBag.WszystkieKategorie = Mapper.Map<List<Event>, List<EventViewModel>>(SelectedEvent);
                }
            }
                using (var db = new SystemContext())
                {
                if ((User.Identity.IsAuthenticated) && (Session["Email"] != null))
                {
                    LogInOperation operation = new LogInOperation();
                    List<Event> events = db.Events.Where(n => n.Category == "Muzyka").ToList();
                    operation.TasteCalculation(events, (string)(Session["Email"]));
                    ViewBag.Muzyka = Mapper.Map<List<Event>, List<EventViewModel>>(events);
                }
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
            LogInOperation operation = new LogInOperation();
            using (var db = new SystemContext())
            {
                List<Event> events;
                if ((User.Identity.IsAuthenticated) && (Session["Email"] != null))
                {
                    events = db.Events.ToList();
                    operation.TasteCalculation(events, (string)(Session["Email"]));
                    ViewBag.test2 = Mapper.Map<List<Event>, List<EventViewModel>>(events);
                }
                else
                {
                    events = db.Events.ToList();
                    ViewBag.test2 = Mapper.Map<List<Event>, List<EventViewModel>>(events);
                }
            }
            return View();
        }
        public ActionResult Test()
        {
            LogInOperation operation = new LogInOperation();
            using (var db = new SystemContext())
            {
                Random rnd = new Random();
                List<Event> RandomEvent = new List<Event>();
                if ((User.Identity.IsAuthenticated) && (Session["Email"] != null))
                {      
                    for (int i = 0; i < 3; i++)
                    {
                        int numId = rnd.Next(1, db.Events.Count());
                        RandomEvent.Add(db.Events.Where(n => n.Id == numId).First());
                    }
                    operation.TasteCalculation(RandomEvent, (string)(Session["Email"]));
                    ViewBag.RandomTest = Mapper.Map<List<Event>, List<EventViewModel>>(RandomEvent);
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        int numId = rnd.Next(1, db.Events.Count());
                        RandomEvent.Add(db.Events.Where(n => n.Id == numId).First());
                    }
                    ViewBag.RandomTest = Mapper.Map<List<Event>, List<EventViewModel>>(RandomEvent);
                }
            }
            return View();
        }
    }
}