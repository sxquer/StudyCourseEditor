using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using StudyCourseEditor.Extensions;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class AnswerController : Controller
    {
        private readonly Entities _db = new Entities();

        /// <summary>
        /// Updates answer
        /// </summary>
        /// <param name="collection">Form data</param>
        /// <param name="id">Answer id to update</param>
        /// <returns>Json result</returns>
        [HttpParamAction]
        public JsonResult Update(FormCollection collection, int id)
        {
            var ajaxResponse = new Dictionary<string, string>();

            string body = collection["Answer"];
            bool isCorrect = (collection["IsCorrect"] != "false");
            Answer answer = _db.Answers.FirstOrDefault(q => q.ID == id);

            try
            {
                CheckAnswerBody(body);
                CheckAnswer(answer);
            }
            catch (AnswerAjaxException ex)
            {
                ajaxResponse["success"] = "false";
                ajaxResponse["message"] = ex.Message;
                return Json(ajaxResponse);
            }

            answer.Body = body;
            answer.IsCorrect = isCorrect;

            _db.ObjectStateManager.ChangeObjectState(answer,
                                                     EntityState.Modified);
            _db.SaveChanges();

            ajaxResponse["message"] = AnswerAjaxMessages.UPDATE_COMPLETE;
            ajaxResponse["actionType"] = "update";
            ajaxResponse["success"] = "true";

            return Json(ajaxResponse);
        }

        /// <summary>
        /// Creates answer
        /// </summary>
        /// <param name="collection">Form data</param>
        /// <param name="id">DEPRICATED</param>
        /// <param name="questionID">Question id which will contain answer</param>
        /// <returns>Json result</returns>
        [HttpParamAction]
        public JsonResult Create(FormCollection collection, int id, int questionID)
        {
            var ajaxResponse = new Dictionary<string, string>();

            string body = collection["Answer"];
            bool isCorrect = (collection["IsCorrect"] != "false");
            var answer = new Answer
                             {
                                 Body = body,
                                 IsCorrect = isCorrect,
                                 QuestionID = questionID
                             };

            try
            {
                CheckAnswerBody(body);
                CheckAnswer(answer);
            }
            catch (AnswerAjaxException ex)
            {
                ajaxResponse["success"] = "false";
                ajaxResponse["message"] = ex.Message;
                return Json(ajaxResponse);
            }

            _db.Answers.AddObject(answer);
            _db.SaveChanges();

            ajaxResponse["message"] = AnswerAjaxMessages.CREATE_COMPLETE;
            ajaxResponse["actionType"] = "create";
            ajaxResponse["success"] = "true";
            ajaxResponse["answerID"] = answer.ID.ToString(CultureInfo.InvariantCulture);
            ajaxResponse["body"] = body;
            ajaxResponse["isCorrect"] = (isCorrect) ? "true" : "false";

            return Json(ajaxResponse);
        }

        /// <summary>
        /// Deletes answer
        /// </summary>
        /// <param name="collection">Form data</param>
        /// <param name="id">Answer id to delete</param>
        /// <returns>Json result</returns>
        [HttpParamAction]
        public JsonResult Delete(FormCollection collection, int id)
        {
            var ajaxResponse = new Dictionary<string, string>();

            Answer answer = _db.Answers.FirstOrDefault(q => q.ID == id);

            try
            {
                CheckAnswer(answer);
            }
            catch (AnswerAjaxException ex)
            {
                ajaxResponse["success"] = "false";
                ajaxResponse["message"] = ex.Message;
                return Json(ajaxResponse);
            }

            _db.Answers.DeleteObject(answer);
            _db.SaveChanges();

            ajaxResponse["message"] = AnswerAjaxMessages.DELETE_COMPLETE;
            ajaxResponse["actionType"] = "delete";
            ajaxResponse["success"] = "true";
            ajaxResponse["answerID"] = id.ToString(CultureInfo.InvariantCulture);

            return Json(ajaxResponse);
        }

        private void CheckAnswerBody(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new AnswerAjaxException(AnswerAjaxMessages.EMPTY_BODY);
        }

        private void CheckAnswer(Answer answer)
        {
            if (answer == null)
                throw new AnswerAjaxException(AnswerAjaxMessages.NULL_ANSWER);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        #region Nested type: AnswerAjaxException

        /// <summary>
        /// Simple wrap around default Exception class
        /// </summary>
        private class AnswerAjaxException : Exception
        {
            public AnswerAjaxException(string message) : base(message)
            {
            }
        }

        #endregion

        #region Nested type: AnswerAjaxMessages

        /// <summary>
        /// Ajax response's list
        /// </summary>
        private struct AnswerAjaxMessages
        {
            public const string EMPTY_BODY = "Тело вопроса не может быть пустым. Изменения не сохранены";
            public const string NULL_ANSWER = "Ответ с заданным ID не найден";
            public const string CREATE_COMPLETE = "Ответ успешно создан";
            public const string UPDATE_COMPLETE = "Ответ успешно сохранен";
            public const string DELETE_COMPLETE = "Ответ успешно удален";
        }

        #endregion
    }
}