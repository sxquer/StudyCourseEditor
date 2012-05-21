using System.Data;
using System.Linq;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class CourseController : Controller
    {
        private readonly Entities _db = new Entities();

        //
        // GET: /Course/

        public ViewResult Index()
        {
            return View(_db.Courses.OrderBy(x => x.Name).ToList());
        }

        //
        // GET: /Course/Details/5

        public ViewResult Details(int id)
        {
            Course course = _db.Courses.FirstOrDefault(c => c.ID == id);
            return View(course);
        }

        //
        // GET: /Course/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Course/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.AddObject(course);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        //
        // GET: /Course/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Course course = _db.Courses.FirstOrDefault(c => c.ID == id);
            return View(course);
        }

        //
        // POST: /Course/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _db.Courses.Attach(course);
                _db.ObjectStateManager.ChangeObjectState(course,
                                                         EntityState.Modified);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        //
        // GET: /Course/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Course course = _db.Courses.FirstOrDefault(c => c.ID == id);
            return View(course);
        }

        //
        // POST: /Course/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = _db.Courses.FirstOrDefault(c => c.ID == id);
            _db.Courses.DeleteObject(course);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public static Course GetById(int id)
        {
            return new Entities().Courses.FirstOrDefault(c => c.ID == id);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}