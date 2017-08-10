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
    public class CoffeesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Coffees
        public ActionResult Index()
        {
            var coffees = db.Coffees.AsQueryable();

            coffees = coffees.OrderBy(x => x.Name).AsQueryable();

            return View(coffees.ToList());
        }

        // GET: Coffees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coffee coffee = db.Coffees.Find(id);
            if (coffee == null)
            {
                return HttpNotFound();
            }
            return View(coffee);
        }

        // GET: Coffees/Create
        public ActionResult Create()
        {
            Coffee coffee = new Coffee();
            ViewBag.Beans = new MultiSelectList(db.Beans.ToList(), "BeanId", "Name", coffee.Beans.Select(x => x.BeanId).ToArray());
            return View(coffee);
        }

        // POST: Coffees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Price, BeanIds")] Coffee coffee, string[] BeanIds)
        {
            if (ModelState.IsValid)
            {
                Coffee checkCoffee = db.Coffees.SingleOrDefault(x => x.Name == coffee.Name);
                if (checkCoffee == null)
                {
                    //coffee.CoffeeId = Guid.NewGuid().ToString();
                    db.Coffees.Add(coffee);
                    db.SaveChanges();

                    if (BeanIds != null)
                    {
                        foreach (string beanId in BeanIds)
                        {
                            Models.CoffeeBean coffeeBean = new Models.CoffeeBean();

                            coffeeBean.CoffeeId = coffee.CoffeeId;
                            coffeeBean.BeanId = beanId;
                            coffee.Beans.Add(coffeeBean);
                        }
                        db.Entry(coffee).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Duplicate Coffee detected.");
                }
            }
            ViewBag.Beans = new MultiSelectList(db.Beans.ToList(), "BeanId", "Name", BeanIds);
            return View(coffee);
        }

        // GET: Coffees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coffee coffee = db.Coffees.Find(id);
            if (coffee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Beans = new MultiSelectList(db.Beans.ToList(), "BeanId", "Name", coffee.Beans.Select(x => x.BeanId).ToArray());
            return View(coffee);
        }

        // POST: Coffees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CoffeeId, Name,Price,BeanIds")] Coffee coffee, string[] BeanIds)
        {
            if (ModelState.IsValid)
            {
                Coffee tmpCoffee = db.Coffees.Find(coffee.CoffeeId);
                if (tmpCoffee != null)
                {
                    Coffee checkgame = db.Coffees.SingleOrDefault(x => x.Name == coffee.Name && x.CoffeeId != coffee.CoffeeId);
                    if (checkgame == null)
                    {
                        tmpCoffee.Name = coffee.Name;
                        tmpCoffee.EditDate = DateTime.Now;
                        tmpCoffee.Price = coffee.Price;

                        db.Entry(tmpCoffee).State = EntityState.Modified;

                        //items to remove
                        var removeItems = tmpCoffee.Beans.Where(x => !BeanIds.Contains(x.BeanId)).ToList();
                        foreach (var removeItem in removeItems)
                        {
                            db.Entry(removeItem).State = EntityState.Deleted;
                        }
                        //items to add
                        if (BeanIds != null)
                        {
                            var addItems = BeanIds.Where(x => !tmpCoffee.Beans.Select(y => y.BeanId).Contains(x));
                            foreach (var addItem in addItems)
                            {
                                Models.CoffeeBean coffeeBean = new Models.CoffeeBean();

                                coffeeBean.CoffeeBeanId = Guid.NewGuid().ToString();
                                coffeeBean.CreateDate = DateTime.Now;
                                coffeeBean.EditDate = coffeeBean.CreateDate;

                                coffeeBean.CoffeeId = tmpCoffee.CoffeeId;
                                coffeeBean.BeanId = addItem;
                                db.CoffeeBeans.Add(coffeeBean);
                            }
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicate Game detected.");
                    }
                }
            }
            //ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", game.GenreId);
            ViewBag.Beans = new MultiSelectList(db.Beans.ToList(), "BeanId", "Name", BeanIds);
            return View(coffee);
        }

        // GET: Coffees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coffee coffee = db.Coffees.Find(id);
            if (coffee == null)
            {
                return HttpNotFound();
            }
            return View(coffee);
        }

        // POST: Coffees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Coffee coffee = db.Coffees.Find(id);
            foreach (var coffeeBean in coffee.Beans.ToList())
            {
                db.CoffeeBeans.Remove(coffeeBean);
            }
            //remove the game
            db.Coffees.Remove(coffee);
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
