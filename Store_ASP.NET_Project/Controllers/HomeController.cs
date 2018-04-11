using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store_ASP.NET_Project.Models;

namespace Store_ASP.NET_Project.Controllers
{
    public class HomeController : Controller
    {
        private StoreDatabase db = new StoreDatabase();
        // GET: Home
        public ActionResult Index()
        {
            if(Session["loggedIn"] == null || (bool)Session["loggedIn"] == false)
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            User ActualUser = db.Users.SingleOrDefault(u => u.Username == user.Username);
            if(ActualUser != null)
            {
                if(ActualUser.Password == user.Password)
                {
                    Session["loggedIn"] = true;
                    return RedirectToAction("Index", "Products", new { area = "" });
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["loggedIn"] = false;
            return RedirectToAction("Login");
        }
    }
}