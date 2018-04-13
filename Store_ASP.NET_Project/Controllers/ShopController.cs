using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store_ASP.NET_Project.Models;

namespace Store_ASP.NET_Project.Controllers
{
    public class ShopController : Controller
    {
        private StoreDatabase db = new StoreDatabase();
        // GET: Shop
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
    }
}