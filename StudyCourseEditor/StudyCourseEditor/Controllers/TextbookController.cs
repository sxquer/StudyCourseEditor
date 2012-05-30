using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class TextbookController : Controller
    {
        private readonly Entities db = new Entities();
        //
        // GET: /Textbook/

        public ActionResult Index()
        {
            return View(db.Courses.OrderBy(x => x.Name).ToList());
        }

        public ActionResult Subject(int id)
        {
            var subject = db.Subjects.FirstOrDefault(x => x.ID == id);
            return View(subject);
        }

    }
}
