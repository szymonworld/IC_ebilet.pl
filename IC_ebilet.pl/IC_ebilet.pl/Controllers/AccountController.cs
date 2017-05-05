using AutoMapper;
using IC_ebilet.pl.Models;
using IC_ebilet.pl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IC_ebilet.pl.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
            CreateDatabase();
            using (var db = new SystemContext())
            {
            }
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View(new UserViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SystemContext())
                {
                    if (!db.Users.Any(n => n.Email == userVM.Email && n.Password == userVM.Password))
                    {
                        return View();
                    }
                    var usr = db.Users.Single(s => s.Email == userVM.Email && s.Password == userVM.Password);
                    if (usr != null)
                    {
                        Session["Email"] = usr.Email.ToString();
                        FormsAuthentication.SetAuthCookie(usr.Email, false);
                        ViewBag.User = usr.Email.ToString();
                        return RedirectToAction("Test", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or Password i wrong");
                    }
                }
            }
            return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Test", "Home");
        }
        public ActionResult LoggedIn()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            return View();
        }
        public ActionResult Register()
        {

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                Category Cat = new Category();
                List<TCategory> category = new List<TCategory>();
                List<TCategory> subcategory = new List<TCategory>();
                foreach (var item in Cat.Categorys.Keys)
                {
                    category.Add((new TCategory { title = item}));
                }
                foreach (var item in Cat.Categorys.Values)
                {
                    foreach (var item2 in item)
                    {
                        subcategory.Add((new TCategory { title = item2 }));
                    }
                }
                Favourite Favnew = new Favourite() { FavCategory = category, SubCategory = subcategory };
                List<int> Banned = new List<int>();
                using (var db = new SystemContext())
                {
                    if (!db.Users.Any(n=>n.Email == userVM.Email))
                    {
                        db.Users.Add(new User { Email = userVM.Email, Password = userVM.Password, Favourite = Favnew, BannedEventID = Banned });
                        db.SaveChanges();
                    }
                }
                ModelState.Clear();
            }
            return View();
        }

        public void CreateDatabase()
        {
            using (var db = new SystemContext())
            {
                db.Database.CreateIfNotExists();
                db.SaveChanges();
            }
        }
        public ActionResult Panel()
        {
            if (!string.IsNullOrEmpty((string)(Session["Email"])))
            {
                using (var db = new SystemContext())
                {
                    string UserEmail = (string)(Session["Email"]);
                    User SelUser = db.Users.Single(n => n.Email == UserEmail);
                    List<TCategory> SelCat = SelUser.Favourite.FavCategory;
                    List<TCategory> SelsubCat = SelUser.Favourite.SubCategory;

                    ViewBag.SubCategory = SelsubCat;
                    return View(Mapper.Map<List<TCategory>, List<TCategoryViewModel>>(SelCat));
                }

                
            }
            return RedirectToAction("index","H");
        }
    }
}