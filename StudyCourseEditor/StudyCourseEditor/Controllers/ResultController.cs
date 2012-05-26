using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{ 
    public class ResultController : Controller
    {
        private Entities db = new Entities();

        public static void Add(Result result)
        {
            var db = new Entities();
            db.Results.AddObject(result);
            db.SaveChanges();
        }

        /*//
        // GET: /Result/

        public ViewResult Index()
        {
            return View(db.Results.ToList());
        }

        //
        // GET: /Result/Details/5

        public ViewResult Details(int id)
        {
            Result result = db.Results.Single(r => r.ID == id);
            return View(result);
        }

        //
        // GET: /Result/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Result/Create

        [HttpPost]
        public ActionResult Create(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.AddObject(result);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(result);
        }
        
        //
        // GET: /Result/Edit/5
 
        public ActionResult Edit(int id)
        {
            Result result = db.Results.Single(r => r.ID == id);
            return View(result);
        }

        //
        // POST: /Result/Edit/5

        [HttpPost]
        public ActionResult Edit(Result result)
        {
            if (ModelState.IsValid)
            {
                db.Results.Attach(result);
                db.ObjectStateManager.ChangeObjectState(result, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(result);
        }

        //
        // GET: /Result/Delete/5
 
        public ActionResult Delete(int id)
        {
            Result result = db.Results.Single(r => r.ID == id);
            return View(result);
        }

        //
        // POST: /Result/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Result result = db.Results.Single(r => r.ID == id);
            db.Results.DeleteObject(result);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }*/
    }
}