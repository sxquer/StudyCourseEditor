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

    public class QuestionController : Controller
    {
        private readonly StudyCourseDB _db = new StudyCourseDB();

        //
        // GET: /Question/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Course");
        }

        //
        // GET: /Question/Details/5

        public ViewResult Details(int id)
        {
            Question question = _db.Questions.Find(id);
            return View(question);
        }

        //
        // GET: /Question/Create

        public ActionResult Create(int subjectId)
        {
            ViewBag.SubjectID = subjectId;
            
            return View();
        } 

        //
        // POST: /Question/Create

        [HttpPost]
        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                _db.Questions.Add(question);
                _db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(question);
        }
        
        //
        // GET: /Question/Edit/5
 
        public ActionResult Edit(int id)
        {
            Question question = _db.Questions.Find(id);
            return View(question);
        }

        //
        // POST: /Question/Edit/5

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(question).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        //
        // GET: /Question/Delete/5
 
        public ActionResult Delete(int id)
        {
            Question question = _db.Questions.Find(id);
            return View(question);
        }

        //
        // POST: /Question/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Question question = _db.Questions.Find(id);
            _db.Questions.Remove(question);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}