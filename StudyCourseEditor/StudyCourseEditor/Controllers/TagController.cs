using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using StudyCourseEditor.Extensions;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{ 
    public class TagController : Controller
    {
        private readonly Entities _db = new Entities();
        
        /// <summary>
        /// Creates DefTagRelation
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <param name="definitionId">Definition id</param>
        /// <returns>Json</returns>
        [HttpParamAction]
        public JsonResult AddToDefinition(string tagName, int definitionId)
        {
            var ajaxResponse = new Dictionary<string, string>();
            ajaxResponse["actionType"] = "create";
            
            int tagId = AddTagToDataBase(tagName);
            var relation =
                _db.DefTagRelations.FirstOrDefault(
                    x => x.DefinitionID == definitionId && x.TagID == tagId);
            
            if (relation != null)
            {
                ajaxResponse["doExist"] = "true";
            }
            else
            {
                ajaxResponse["doExist"] = "false";
                relation = new DefTagRelation
                {
                    DefinitionID = definitionId,
                    TagID = tagId,
                };

                _db.DefTagRelations.AddObject(relation);
                _db.SaveChanges(); 
            }

            ajaxResponse["id"] = tagId.ToString(CultureInfo.InvariantCulture);
            ajaxResponse["message"] = "Тэг успешно добавлен";
            ajaxResponse["success"] = "true";
            ajaxResponse["body"] = tagName;
            return Json(ajaxResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes DefTagRelation
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="definitionId">Definition id</param>
        /// <returns>Json</returns>
        [HttpParamAction]
        public JsonResult RemoveFromDefinition(int tagId, int definitionId)
        {
            var ajaxResponse = new Dictionary<string, string>();
            ajaxResponse["actionType"] = "delete";

            var allRelationsWithTag = _db.DefTagRelations.Where(x => x.TagID == tagId);
            var relation = (allRelationsWithTag.Any())
                               ? allRelationsWithTag.FirstOrDefault(
                                   x => x.DefinitionID == definitionId)
                               : null;
            
            if (relation == null)
            {
                ajaxResponse["message"] = "Тэг не найден";
                ajaxResponse["success"] = "false";
                return Json(ajaxResponse, JsonRequestBehavior.AllowGet);
            }

            RemoveDefFromDataBase(relation);
            if (allRelationsWithTag.Count() == 1) RemoveTagFromDataBase(tagId);

            ajaxResponse["id"] = tagId.ToString(CultureInfo.InvariantCulture);
            ajaxResponse["message"] = "Тэг удален";
            ajaxResponse["success"] = "true";
            return Json(ajaxResponse, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Creates Tag in database
        /// </summary>
        /// <param name="tagName">Tag name</param>
        /// <returns>Tag id</returns>
        private int AddTagToDataBase(string tagName)
        {
            var tag = _db.Tags.FirstOrDefault(x => x.Name == tagName);
            if (tag == null)
            {
                tag = new Tag {Name = tagName};
                _db.Tags.AddObject(tag);
                _db.SaveChanges();
            }
            return tag.ID;
        }

        /// <summary>
        /// Deletes DefTagRelation from database
        /// </summary>
        /// <param name="relation"></param>
        private void RemoveDefFromDataBase(DefTagRelation relation)
        {
            _db.DefTagRelations.DeleteObject(relation);
            _db.SaveChanges();
        }


        /// <summary>
        /// Deletes Tag from database
        /// </summary>
        /// <param name="tagId">Tag id</param>
        private void RemoveTagFromDataBase(int tagId)
        {
            var tag = _db.Tags.FirstOrDefault(x => x.ID == tagId);
            _db.Tags.DeleteObject(tag);
            _db.SaveChanges();
        }

        /// <summary>
        /// Returns list of tag names contains mask 
        /// </summary>
        /// <param name="mask">Mask (seeking substring)</param>
        /// <returns>Json</returns>
        public JsonResult GetTagList(string mask)
        {
            var ajaxResponse = _db.Tags.Where(x => x.Name.Contains(mask)).Select(x => x.Name).ToList();
            return Json(ajaxResponse, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
           base.Dispose(disposing);
        }
    }
}