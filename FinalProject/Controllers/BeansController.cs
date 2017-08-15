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
    [Authorize]
    public class BeansController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Beans
        [AllowAnonymous]
        public ActionResult Index()
        {
            var beans = db.Beans.AsQueryable();

            beans = beans.OrderBy(x => x.Name).AsQueryable();

            return View(beans.ToList());
        }

        // GET: Beans/Details/5
        [AllowAnonymous]
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
            Bean bean = new Bean();
            ViewBag.Coffees = new MultiSelectList(db.Coffees.ToList(), "CoffeeId", "Name", bean.Coffees.Select(x => x.CoffeeId).ToArray());
            return View(bean);
        }

        // POST: Beans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Price,CoffeeIds")] Bean bean, string[] CoffeeIds)
        {
            if (ModelState.IsValid)
            {
                Bean checkbean = db.Beans.SingleOrDefault(x => x.Name == bean.Name);
                if (checkbean == null)
                {
                    db.Beans.Add(bean);
                    db.SaveChanges();
                }
                if (CoffeeIds != null)
                {

                    foreach (string coffeeId in CoffeeIds)
                    {
                        Models.CoffeeBean coffeeBean = new Models.CoffeeBean();
                        
                        coffeeBean.BeanId = bean.BeanId;
                        coffeeBean.CoffeeId = coffeeId;
                        bean.Coffees.Add(coffeeBean);
                    }
                    db.Entry(bean).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Duplicate Bean detected.");
            }
            ViewBag.Coffees = new MultiSelectList(db.Coffees.ToList(), "CoffeeId", "Name", CoffeeIds);
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
            ViewBag.Coffees = new MultiSelectList(db.Coffees.ToList(), "CoffeeId", "Name", bean.Coffees.Select(x => x.CoffeeId).ToArray());
            return View(bean);
        }

        // POST: Beans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeanId,Name,Price,CoffeeIds")] Bean bean, string[] CoffeeIds)
        {
            if (ModelState.IsValid)
            {
                Bean tmpBean = db.Beans.Find(bean.BeanId);
                if (tmpBean != null)
                {
                    Bean checkBean = db.Beans.SingleOrDefault(x => x.Name == bean.Name && x.BeanId != bean.BeanId);
                    if (checkBean == null)
                    {
                        tmpBean.Name = bean.Name;
                        tmpBean.EditDate = DateTime.Now;
                        tmpBean.Price = bean.Price;

                        db.Entry(tmpBean).State = EntityState.Modified;

                        //items to remove
                        var removeItems = tmpBean.Coffees.Where(x => !CoffeeIds.Contains(x.CoffeeId)).ToList();
                        foreach (var removeItem in removeItems)
                        {
                            db.Entry(removeItem).State = EntityState.Deleted;
                        }
                        //items to add
                        if (CoffeeIds != null)
                        {
                            var addItems = CoffeeIds.Where(x => !tmpBean.Coffees.Select(y => y.CoffeeId).Contains(x));
                            foreach (var addItem in addItems)
                            {
                                Models.CoffeeBean coffeeBean = new Models.CoffeeBean();

                                coffeeBean.CoffeeBeanId = Guid.NewGuid().ToString();
                                coffeeBean.CreateDate = DateTime.Now;
                                coffeeBean.EditDate = coffeeBean.CreateDate;

                                coffeeBean.BeanId = tmpBean.BeanId;
                                coffeeBean.CoffeeId = addItem;
                                db.CoffeeBeans.Add(coffeeBean);
                            }
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicate Bean detected.");
                    }
                }
            }
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", game.GenreId);
            ViewBag.Genres = new MultiSelectList(db.Coffees.ToList(), "CoffeeId", "Name", CoffeeIds);
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
            foreach (var coffeeBean in bean.Coffees.ToList())
            {
                db.CoffeeBeans.Remove(coffeeBean);
            }
            db.Beans.Remove(bean);
            var removed = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
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
