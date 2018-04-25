using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Store_ASP.NET_Project.Models;

namespace Store_ASP.NET_Project.Controllers
{
    public class ShopController : Controller
    {
        private StoreDatabase db = new StoreDatabase();
        // GET: Shop
        [HttpGet]
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

            double avg = db.Reviews.Where(x => x.ProductId == id).Select(x => (double?)x.Stars).Average() ?? 0.0;
            ViewData["AvgStars"] = avg;

            ViewData["Reviews"] = db.Reviews.Where(x => x.ProductId == id).ToList();



            string userName = Session["userName"] != null ? Session["userNam"].ToString() : "";
            List<Review> reviewList = db.Reviews.Where(x => x.UserName == userName && x.ProductId == id).ToList();

            ViewData["UserReview"] = reviewList.Count > 0 ? reviewList.First() : new Review();

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

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            string dept = col["dept"];
            string term = col["term"];
            ViewBag.Cart = TempData["Cart"];
            ViewBag.Categories = db.Products.ToList().Select(x => x.Category).Distinct();
            TempData.Keep("Cart");
            List<Product> results = new List<Product>();
            if (dept != "All Categories")
            {
                results = db.Products.Where(item => item.Category.ToLower() == dept.ToString().ToLower()).ToList();
            }
            else
            {
                results = db.Products.ToList();
            }
            results = results.Where(item => Regex.IsMatch(item.Name.ToLower(), ".*" + term.ToLower().Trim() + ".*")).ToList();
            return View(results);
        }

        public ActionResult WishListAdd(int? id)
        {
            WishList wish = new WishList();
            int nId = id.Value;
            wish.ProductId = nId;
            wish.UserId = (int)Session["userID"];
            db.WishLists.Add(wish);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost, ActionName("Details")]
        public ActionResult ReviewAdd(FormCollection col)
        {
            int id = Convert.ToInt32(col["id"]);
            int productId = Convert.ToInt32(col["productId"]);
            int stars = Convert.ToInt32(col["starsInput"]);
            string subject = col["subjectInput"];
            string comment = col["commentInput"];
            if (id == -1)
            {
                db.Reviews.Add(new Review(stars, subject, comment, Session["userName"].ToString(), productId));
            }
            else
            {
                Review rev = db.Reviews.Where(i => i.Id == id).Single();
                rev.Stars = stars;
                rev.Subject = subject;
                rev.Comment = comment;
            }
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}