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

    }
}
