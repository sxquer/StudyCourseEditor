using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class SubjectController : Controller
    {
        StudyCourseDB _db = new StudyCourseDB();

        
        public ActionResult Index(string courseName, string subject)
        {
            return View();
        }

        public ActionResult List(string courseName)
        {
            var course = _db.Courses.FirstOrDefault(x => x.Name == courseName);
            if (course == null) return RedirectToAction("Index", "Home");
            
            var model = _db.Subjects.Where(x => x.Course.Name == courseName);

            ViewBag.CourseName = course.Name;
            ViewBag.CourseDescription = course.Description;
            ViewBag.isUserAdmin = AccountController.IsUserAdmin();
            
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(Subject subject)
        {
            _db.Subjects.Add(subject);
            _db.SaveChanges();

            return RedirectToAction("List");
        }
        
        
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            _db.Subjects.Remove(_db.Subjects.Find(id));
            _db.SaveChanges();
            
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var model = _db.Subjects.Find(id);
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var subject = _db.Subjects.Find(id);

            if (TryUpdateModel(subject, collection))
            {
                _db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(subject);
        }

        public ActionResult Details(int id)
        {
            var model = _db.Subjects.Find(id);
            return View(model);
        }

    }
}
