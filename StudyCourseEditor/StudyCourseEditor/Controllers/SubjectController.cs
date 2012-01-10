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
            ViewBag.isUserAdmin = AccountController.isUserAdmin();
            
            return View(model);
        }

    }
}
