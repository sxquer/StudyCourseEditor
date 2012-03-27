using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Под конструкцией.";

            var test = new Tag();
            test.DefTagRelations.Select(x => x.Definition);

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
