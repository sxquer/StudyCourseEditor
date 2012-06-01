using System;
using System.Web.Mvc;
using System.Web.Security;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}