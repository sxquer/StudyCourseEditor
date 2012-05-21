using System.Data;
using System.Linq;
using System.Web.Mvc;
using StudyCourseEditor.Extensions;
using StudyCourseEditor.Models;
using Question = StudyCourseEditor.Models.Question;

namespace StudyCourseEditor.Controllers
{
    public class QuestionController : Controller
    {
        private readonly Entities _db = new Entities();

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
            Question question = _db.Questions.FirstOrDefault(q => q.ID == id);
            return View(question);
        }

        //
        // GET: /Question/Create

        public ActionResult Create(int subjectId)
        {
            ViewBag.SubjectID = subjectId;
            ViewBag.Types = GetQuestionTypes();

            return View();
        }

        //
        // POST: /Question/Create
        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateAndBack(Question question)
        {
            if (ModelState.IsValid)
            {
                _db.Questions.AddObject(question);
                _db.SaveChanges();
                return RedirectToAction("Edit", "Subject",
                                        new {id = question.SubjectID});
            }

            ViewBag.Types = GetQuestionTypes();
            return View("Create");
        }

        //
        // POST: /Question/Create
        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateAndContinue(Question question)
        {
            if (ModelState.IsValid)
            {
                _db.Questions.AddObject(question);
                _db.SaveChanges();
                return RedirectToAction("Edit", "Question",
                                        new {id = question.ID});
            }

            ViewBag.Types = GetQuestionTypes();
            return View("Create");
        }

        //
        // GET: /Question/Edit/5

        public ActionResult Edit(int id)
        {
            Question question = _db.Questions.FirstOrDefault(q => q.ID == id);
            ViewBag.Types = GetQuestionTypes();
            return View(question);
        }

        //
        // POST: /Question/Edit/5

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                _db.Questions.Attach(question);
                _db.ObjectStateManager.ChangeObjectState(question,
                                                         EntityState.Modified);
                _db.SaveChanges();
                //return RedirectToAction("Edit", "Subject",new {id = question.SubjectID});
            }
            ViewBag.Types = GetQuestionTypes();
            return View(question);
        }

        //
        // GET: /Question/Delete/5

        public ActionResult Delete(int id)
        {
            Question question = _db.Questions.FirstOrDefault(q => q.ID == id);
            return View(question);
        }

        //
        // POST: /Question/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = _db.Questions.FirstOrDefault(q => q.ID == id);
            _db.Questions.DeleteObject(question);
            _db.SaveChanges();
            if (question != null)
                return RedirectToAction("Edit", "Subject",
                                        new {id = question.SubjectID});
            return RedirectToAction("Index");
        }

        private SelectList GetQuestionTypes()
        {
            return new SelectList(_db.QuestionTypes, "ID", "Name");
        }

        public ActionResult QuestionTypeAjaxHelp(int typeID)
        {
            var type = _db.QuestionTypes.FirstOrDefault(qt => qt.ID == typeID);
            if (type == null) return Json("Подсказка не найдена", JsonRequestBehavior.AllowGet);
            return Json(type.Description, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}