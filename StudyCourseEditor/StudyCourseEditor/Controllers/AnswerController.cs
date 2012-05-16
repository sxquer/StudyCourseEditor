using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{ 
    public class AnswerController : Controller
    {
        private Entities db = new Entities();

        //
        // POST: /Answer/Create

        private bool Create(Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Answers.AddObject(answer);
                db.SaveChanges();
                return true;
            }

            return false;
        }
        

        //
        // POST: /Answer/Edit/5

        private bool Update(Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.ObjectStateManager.ChangeObjectState(answer, EntityState.Modified);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        private bool Delete(Answer answer)
        {
            if (answer != null)
            {
                db.Answers.DeleteObject(answer);
                db.SaveChanges();
                return true;
            }

            return false;
        }
        
        public JsonResult ProcessAnswer(FormCollection collection, int id, int counter, int questionID = 0)
        {
            var ajaxResponse = new string[4];
            string body = collection["Answer_" + counter];
            bool isCorrect = (collection["IsCorrect_" + counter] == "true");

            ajaxResponse[0] = id.ToString(CultureInfo.InvariantCulture);
            ajaxResponse[3] = "0";
            
            var req = ControllerContext.RequestContext.HttpContext.Request;
            ajaxResponse[1] = req.Form["submit"];

            if (ajaxResponse[1] == "Добавить")
            {
                Create(new Answer
                {
                    Body = body,
                    IsCorrect = false,
                    QuestionID = questionID
                });
                ajaxResponse[2] = "Ответ успешно создан";
                return Json(ajaxResponse);
            }

            Answer answer = db.Answers.FirstOrDefault(q => q.ID == id);
            if (answer == null)
            {
                ajaxResponse[2] = "Ответ с заданным ID не найден";
                ajaxResponse[3] = "1";
                return Json(ajaxResponse);
            }

            if (ajaxResponse[1] == "Сохранить")
            {
                answer.Body = body;
                answer.IsCorrect = isCorrect;
                Update(answer);
                ajaxResponse[2] = "Ответ сохранен";
                return Json(ajaxResponse);
            }

            if (ajaxResponse[1] == "Удалить")
            {
                Delete(answer);
                ajaxResponse[2] = "Ответ удален";
                return Json(ajaxResponse);
            }

            ajaxResponse[2] = "Команда не найдена";
            ajaxResponse[3] = "1";
            return Json(ajaxResponse);

        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}