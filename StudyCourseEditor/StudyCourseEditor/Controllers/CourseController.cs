using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class CourseController : Controller
    {
        StudyCourseDB _db = new StudyCourseDB();

        
        public ActionResult Index()
        {
            var model = _db.Courses;

            ViewBag.isUserAdmin = Roles.GetRolesForUser().Contains("administrator");

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(Course course)
        {
            _db.Courses.Add(course);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var model = _db.Courses.Find(id);

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var course = _db.Courses.Find(id);
            
            if (TryUpdateModel(course, collection))
            {
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            _db.Courses.Remove(_db.Courses.Find(id));
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int id)
        {
            var model = _db.Courses.Find(id);

            return View(model);
        }

    }
}
