using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Store_ASP.NET_Project.Models;

namespace Store_ASP.NET_Project.Controllers
{
    public class WishListsController : Controller
    {
        private StoreDatabase db = new StoreDatabase();

        // GET: WishLists
        public ActionResult Index()
        {

            List<Product> products = new List<Product>();
            if (Session["userID"] != null)
            {
                ArrayList myData = new ArrayList();
                List<WishList> wishes;
                int userID = (int)Session["userID"];
                wishes = db.WishLists.Where(item => item.UserId == userID).ToList();
                foreach (WishList wish in wishes)
                {
                    Product product = new Product();
                    product = db.Products.Where(i => i.Id == wish.ProductId).Single();
                    product.Stock = wish.Id;
                    products.Add(product);
                }
            }

            return View(products);
        }

        // GET: WishLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product  product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", "Details", new { area = "Products" });
        }


        // GET: WishLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList wishList = db.WishLists.Find(id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishList wishList = db.WishLists.Find(id);
            db.WishLists.Remove(wishList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
