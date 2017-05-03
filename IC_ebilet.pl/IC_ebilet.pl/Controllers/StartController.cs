using IC_ebilet.pl.Models;
using IC_ebilet.pl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IC_ebilet.pl.Controllers
{
    public class StartController : Controller
    {
        List<User> UsersList = new List<User>();
        public void CreateDatabase()
        {
            using (var db = new SystemContext())
            {

               db.Database.CreateIfNotExists();
                var r = db.Users.FirstOrDefault();
                UsersList = db.Users.ToList();
                db.SaveChanges();
            }
        }
        // GET: Start
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Log()
        {
            CreateDatabase();
            using (var db = new SystemContext())
            {
                    UsersList = db.Users.ToList();   
            }
            return View();
        }
        public ActionResult UsersTable()
        {
            using (var db = new SystemContext())
            {
                UsersList = db.Users.ToList();
            }
            ViewBag.usery = UsersList;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            using (var db = new SystemContext())
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        [HttpPost]
        public ActionResult LogIn(User model)
        {
            using (var db = new SystemContext())
            {
                if (true)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();


        }
        public ActionResult Register(User model)
        {
            using (var db = new SystemContext())
            {
                db.Users.Add(model);
                db.SaveChanges();
            }
           // return RedirectToAction("Index", "Home");
            return RedirectToAction("Log", "Start");

        }

    }
}