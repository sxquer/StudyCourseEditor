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
    public class SubjectController : Controller
    {
        private readonly Entities _db = new Entities(); 

        //
        // GET: /Subject/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Course");
        }

        //
        // GET: /Subject/Details/5

        public ViewResult Details(int id)
        {
            Subject subject = _db.Subjects.FirstOrDefault(s => s.ID == id);
            return View(subject);
        }

        //
        // GET: /Subject/Create

        public ActionResult Create(int courseId)
        {
            ViewBag.CourseID = courseId;
            return View();
        } 

        //
        // POST: /Subject/Create

        [HttpPost]
        public ActionResult Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                _db.Subjects.AddObject(subject);
                _db.SaveChanges();
                return RedirectToAction("Edit", "Course", new { id = subject.CourseID }); 
            }

            ViewBag.CourseID = subject.CourseID;
            return View(subject);
        }
        
        //
        // GET: /Subject/Edit/5
 
        public ActionResult Edit(int id)
        {
            Subject subject = _db.Subjects.FirstOrDefault(s => s.ID == id);
            return View(subject);
        }

        //
        // POST: /Subject/Edit/5

        [HttpPost]
        public ActionResult Edit(Subject subject)
        {
            if (ModelState.IsValid)
            {
                _db.Subjects.Attach(subject);
                _db.ObjectStateManager.ChangeObjectState(subject, EntityState.Modified);
                _db.SaveChanges();
                return RedirectToAction("Edit", "Course", new { id = subject.CourseID }); 
            }
            
            return View(subject);
        }

        //
        // GET: /Subject/Delete/5
 
        public ActionResult Delete(int id)
        {
            Subject subject = _db.Subjects.FirstOrDefault(s => s.ID == id);
            return View(subject);
        }

        //
        // POST: /Subject/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Subject subject = _db.Subjects.FirstOrDefault(s => s.ID == id);
            _db.Subjects.DeleteObject(subject);
            _db.SaveChanges();
            if (subject != null) return RedirectToAction("Edit", "Course", new { id = subject.CourseID });
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}