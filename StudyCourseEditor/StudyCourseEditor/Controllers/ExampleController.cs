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
    public class ExampleController : Controller
    {
        private readonly Entities _db = new Entities();

        /// <summary>
        /// Updates example
        /// </summary>
        /// <param name="collection">Form data</param>
        /// <param name="id">Example id to update</param>
        /// <returns>Json result</returns>
        [HttpParamAction]
        public JsonResult Update(FormCollection collection, int id)
        {
            var ajaxResponse = new Dictionary<string, string>();

            string body = collection["Example"];
            Example example = _db.Examples.FirstOrDefault(q => q.ID == id);

            try
            {
                CheckExampleBody(body);
                CheckExamlpe(example);
            }
            catch (ExampleAjaxException ex)
            {
                ajaxResponse["success"] = "false";
                ajaxResponse["message"] = ex.Message;
                return Json(ajaxResponse);
            }

            example.Value = body;

            _db.ObjectStateManager.ChangeObjectState(example,
                                                     EntityState.Modified);
            _db.SaveChanges();

            ajaxResponse["message"] = ExampleAjaxMessages.UPDATE_COMPLETE;
            ajaxResponse["actionType"] = "update";
            ajaxResponse["success"] = "true";

            return Json(ajaxResponse);
        }

        /// <summary>
        /// Creates example
        /// </summary>
        /// <param name="collection">Form data</param>
        /// <param name="id">DEPRICATED</param>
        /// <param name="questionID">Example id which will contain answer</param>
        /// <param name="isAntiExample"></param>
        /// <returns>Json result</returns>
        [HttpParamAction]
        public JsonResult Create(FormCollection collection, int id, int definitionId, bool isAntiExample)
        {
            var ajaxResponse = new Dictionary<string, string>();

            string body = collection["Example"];
            var example = new Example
            {
                Value = body,
                IsAntiExample = isAntiExample,
                DefinitionID = definitionId
            };

            try
            {
                CheckExampleBody(body);
                CheckExamlpe(example);
            }
            catch (ExampleAjaxException ex)
            {
                ajaxResponse["success"] = "false";
                ajaxResponse["message"] = ex.Message;
                return Json(ajaxResponse);
            }

            _db.Examples.AddObject(example);
            _db.SaveChanges();

            ajaxResponse["message"] = ExampleAjaxMessages.CREATE_COMPLETE;
            ajaxResponse["actionType"] = "create";
            ajaxResponse["success"] = "true";
            ajaxResponse["exampleID"] = example.ID.ToString(CultureInfo.InvariantCulture);
            ajaxResponse["body"] = body;
            ajaxResponse["isAntiExample"] = (isAntiExample) ? "true" : "false";

            return Json(ajaxResponse);
        }

        /// <summary>
        /// Deletes examples
        /// </summary>
        /// <param name="collection">Form data</param>
        /// <param name="id">Example id to delete</param>
        /// <returns>Json result</returns>
        [HttpParamAction]
        public JsonResult Delete(FormCollection collection, int id)
        {
            var ajaxResponse = new Dictionary<string, string>();

            Example example = _db.Examples.FirstOrDefault(q => q.ID == id);

            try
            {
                CheckExamlpe(example);
            }
            catch (ExampleAjaxException ex)
            {
                ajaxResponse["success"] = "false";
                ajaxResponse["message"] = ex.Message;
                return Json(ajaxResponse);
            }

            _db.Examples.DeleteObject(example);
            _db.SaveChanges();

            ajaxResponse["message"] = ExampleAjaxMessages.DELETE_COMPLETE;
            ajaxResponse["actionType"] = "delete";
            ajaxResponse["success"] = "true";
            ajaxResponse["exampleID"] = id.ToString(CultureInfo.InvariantCulture);

            return Json(ajaxResponse);
        }

        /// <summary>
        /// Check if example body is correct
        /// </summary>
        /// <param name="body"></param>
        private void CheckExampleBody(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                throw new ExampleAjaxException(ExampleAjaxMessages.EMPTY_BODY);
        }


        /// <summary>
        /// Check if example is not null
        /// </summary>
        /// <param name="example"></param>
        private void CheckExamlpe(Example example)
        {
            if (example == null)
                throw new ExampleAjaxException(ExampleAjaxMessages.NULL_EXAMPLE);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        #region Nested type: ExampleAjaxException

        /// <summary>
        /// Simple wrap around default Exception class
        /// </summary>
        private class ExampleAjaxException : Exception
        {
            public ExampleAjaxException(string message)
                : base(message)
            {
            }
        }

        #endregion

        #region Nested type: ExampleAjaxException

        /// <summary>
        /// Ajax response's list
        /// </summary>
        private struct ExampleAjaxMessages
        {
            public const string EMPTY_BODY = "Тело примера не может быть пустым. Изменения не сохранены";
            public const string NULL_EXAMPLE = "Пример с заданным ID не найден";
            public const string CREATE_COMPLETE = "Ответ успешно создан";
            public const string UPDATE_COMPLETE = "Ответ успешно сохранен";
            public const string DELETE_COMPLETE = "Ответ успешно удален";
        }

        #endregion
    }
}