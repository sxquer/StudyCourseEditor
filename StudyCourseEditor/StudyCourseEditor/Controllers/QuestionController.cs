using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using StudyCourseEditor.Extensions;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class QuestionController : Controller
    {
        private readonly Entities _db = new Entities();

        #region CRUD

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
            return Create(question, "Subject");
        }

        //
        // POST: /Question/Create
        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateAndContinue(Question question)
        {
            return Create(question, "Question");
        }

        /// <summary>
        /// Creates question
        /// </summary>
        /// <param name="question"></param>
        /// <param name="controller">Controller name for redirection</param>
        /// <returns></returns>
        private ActionResult Create(Question question, string controller)
        {
            if (ModelState.IsValid)
            {
                _db.Questions.AddObject(question);
                _db.SaveChanges();
                return RedirectToAction("Edit", controller,
                                        new { id = question.ID });
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
                _db.ObjectStateManager.ChangeObjectState(question,EntityState.Modified);
                _db.SaveChanges();
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

        #endregion

        /// <summary>
        /// Returns SelectList of qustion types
        /// </summary>
        /// <returns></returns>
        private SelectList GetQuestionTypes()
        {
            return new SelectList(_db.QuestionTypes, "ID", "Name");
        }

        /// <summary>
        /// Get question or null by id
        /// </summary>
        /// <param name="id">Question id</param>
        /// <returns></returns>
        public static Question GetById(int id)
        {
            return new Entities().Questions.FirstOrDefault(x => x.ID == id);
        }

        /// <summary>
        /// Saves information about student's results on specific question
        /// </summary>
        /// <param name="questionId">Question id</param>
        /// <param name="isCorrect">Answer is correct</param>
        public static void AddAttempt(int questionId, bool isCorrect)
        {
            var db = new Entities();

            var question = db.Questions.FirstOrDefault(x => x.ID == questionId);

            if (question == null) throw new Exception("Вопрос не найден");

            question.TotalAttempts++;
            if (isCorrect) question.RightAttempts++;

            db.ObjectStateManager.ChangeObjectState(question, EntityState.Modified);
            db.SaveChanges();
        }

        /// <summary>
        /// Returns ajax representation of question type description
        /// </summary>
        /// <param name="typeID">Question type id</param>
        /// <returns></returns>
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