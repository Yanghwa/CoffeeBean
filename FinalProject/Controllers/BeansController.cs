using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class BeansController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Beans
        public ActionResult Index()
        {
            return View(db.Beans.ToList());
        }

        // GET: Beans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bean bean = db.Beans.Find(id);
            if (bean == null)
            {
                return HttpNotFound();
            }
            return View(bean);
        }

        // GET: Beans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Beans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BeanId,Name,Price,CreateDate,EditDate")] Bean bean)
        {
            if (ModelState.IsValid)
            {
                db.Beans.Add(bean);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bean);
        }

        // GET: Beans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bean bean = db.Beans.Find(id);
            if (bean == null)
            {
                return HttpNotFound();
            }
            return View(bean);
        }

        // POST: Beans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeanId,Name,Price,CreateDate,EditDate")] Bean bean)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bean).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bean);
        }

        // GET: Beans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bean bean = db.Beans.Find(id);
            if (bean == null)
            {
                return HttpNotFound();
            }
            return View(bean);
        }

        // POST: Beans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Bean bean = db.Beans.Find(id);
            db.Beans.Remove(bean);
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
