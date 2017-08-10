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
    public class CoffeeBeansController : Controller
    {
        private DataContext db = new DataContext();

        // GET: CoffeeBeans
        public ActionResult Index()
        {
            var coffeeBeans = db.CoffeeBeans.Include(c => c.Bean).Include(c => c.Coffee);
            return View(coffeeBeans.ToList());
        }

        // GET: CoffeeBeans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CoffeeBean coffeeBean = db.CoffeeBeans.Find(id);
            if (coffeeBean == null)
            {
                return HttpNotFound();
            }
            return View(coffeeBean);
        }

        // GET: CoffeeBeans/Create
        public ActionResult Create()
        {
            ViewBag.BeanId = new SelectList(db.Beans, "BeanId", "Name");
            ViewBag.CoffeeId = new SelectList(db.Coffees, "CoffeeId", "Name");
            return View();
        }

        // POST: CoffeeBeans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CoffeeBeanId,CoffeeId,BeanId,CreateDate,EditDate")] Models.CoffeeBean coffeeBean)
        {
            if (ModelState.IsValid)
            {
                Models.CoffeeBean tmpCoffeeBean = db.CoffeeBeans.SingleOrDefault(y => y.BeanId == coffeeBean.BeanId && y.CoffeeId == coffeeBean.CoffeeId && y.CoffeeBeanId == coffeeBean.CoffeeBeanId);
                if (tmpCoffeeBean == null)
                {
                    coffeeBean.CoffeeBeanId = Guid.NewGuid().ToString();
                    coffeeBean.CreateDate = DateTime.Now;
                    coffeeBean.EditDate = coffeeBean.CreateDate;
                    db.CoffeeBeans.Add(coffeeBean);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate entry found");
                }
            }

            ViewBag.BeanId = new SelectList(db.Beans, "BeanId", "Name", coffeeBean.BeanId);
            ViewBag.CoffeeId = new SelectList(db.Coffees, "CoffeeId", "Name", coffeeBean.CoffeeId);
            return View(coffeeBean);
        }

        // GET: CoffeeBeans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CoffeeBean coffeeBean = db.CoffeeBeans.Find(id);
            if (coffeeBean == null)
            {
                return HttpNotFound();
            }
            ViewBag.BeanId = new SelectList(db.Beans, "BeanId", "Name", coffeeBean.BeanId);
            ViewBag.CoffeeId = new SelectList(db.Coffees, "CoffeeId", "Name", coffeeBean.CoffeeId);
            return View(coffeeBean);
        }

        // POST: CoffeeBeans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CoffeeBeanId,CoffeeId,BeanId,CreateDate,EditDate")] Models.CoffeeBean coffeeBean)
        {
            if (ModelState.IsValid)
            {
                var tmpCoffeeBean = db.CoffeeBeans.Find(coffeeBean.CoffeeBeanId);
                tmpCoffeeBean.EditDate = DateTime.Now;
                tmpCoffeeBean.BeanId = coffeeBean.BeanId;
                tmpCoffeeBean.CoffeeId = coffeeBean.CoffeeId;
                db.Entry(tmpCoffeeBean).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BeanId = new SelectList(db.Beans, "BeanId", "Name", coffeeBean.BeanId);
            ViewBag.CoffeeId = new SelectList(db.Coffees, "CoffeeId", "Name", coffeeBean.CoffeeId);
            return View(coffeeBean);
        }

        // GET: CoffeeBeans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.CoffeeBean coffeeBean = db.CoffeeBeans.Find(id);
            if (coffeeBean == null)
            {
                return HttpNotFound();
            }
            return View(coffeeBean);
        }

        // POST: CoffeeBeans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Models.CoffeeBean coffeeBean = db.CoffeeBeans.Find(id);
            db.CoffeeBeans.Remove(coffeeBean);
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
