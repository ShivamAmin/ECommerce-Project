using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            ViewBag.Cart = TempData["Cart"];
            ViewBag.Categories = db.Products.ToList().Select(x => x.Category).Distinct();
            TempData.Keep("Cart");
            return View(db.Products.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            TempData.Keep("Cart");
            return View(product);
        }
        public ActionResult Add(int? id)
        {
            List<Product> cartList = new List<Product>();
            if (TempData["Cart"] != null)
            {
                cartList = (List<Product>)TempData["Cart"];
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            cartList.Add(product);
            TempData["Cart"] = cartList;
            TempData.Keep("Cart");
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Delete(int? id)
        {
            List<Product> cartList = (List<Product>)TempData["Cart"];
            cartList.Remove(cartList.Where(item => item.Id == id).First());
            return Redirect(Request.UrlReferrer.ToString());
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            return View(TempData.Peek("Cart"));
        }
        [HttpPost, ActionName("Checkout")]
        public ActionResult CheckoutConfirmed()
        {
            TempData.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}