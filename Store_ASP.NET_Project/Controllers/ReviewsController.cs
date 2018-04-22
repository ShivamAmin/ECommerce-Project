using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Store_ASP.NET_Project.Models;

namespace Store_ASP.NET_Project.Controllers
{
    public class ReviewsController : Controller
    {
        private StoreDatabase db = new StoreDatabase();

        // GET: Reviews
        public ActionResult Index()
        {
            //add lists by category in viewBag [refer to petshop lab 7]
            ViewBag.productCategory = new SelectList(db.ReviewTbls,"productCategory","productCategory");
            List<ReviewTbl> reviewTblList = db.ReviewTbls.Where(item => item.productCategory == "Electronics").ToList();
            return View(reviewTblList);

        }

        [HttpPost]
        public ActionResult Index(string productCategory)
        {
            
            List<ReviewTbl> reviewTblList =  db.ReviewTbls.Where(item => item.productCategory == productCategory).ToList();
            return View(reviewTblList);
        }


        // GET: Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewTbl reviewTbl = db.ReviewTbls.Find(id);
            if (reviewTbl == null)
            {
                return HttpNotFound();
            }
            return View(reviewTbl);
        }

        // GET: Reviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,productCategory,UsernameComment,Subject,Comment")] ReviewTbl reviewTbl)
        {
            if (ModelState.IsValid)
            {
                db.ReviewTbls.Add(reviewTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reviewTbl);
        }

        // GET: Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewTbl reviewTbl = db.ReviewTbls.Find(id);
            if (reviewTbl == null)
            {
                return HttpNotFound();
            }
            return View(reviewTbl);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,productCategory,UsernameComment,Subject,Comment")] ReviewTbl reviewTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reviewTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reviewTbl);
        }

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReviewTbl reviewTbl = db.ReviewTbls.Find(id);
            if (reviewTbl == null)
            {
                return HttpNotFound();
            }
            return View(reviewTbl);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReviewTbl reviewTbl = db.ReviewTbls.Find(id);
            db.ReviewTbls.Remove(reviewTbl);
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
