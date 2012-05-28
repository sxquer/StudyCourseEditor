using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Extensions;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{ 
    public class TagController : Controller
    {
        private Entities db = new Entities();
        [HttpParamAction]
        public JsonResult AddToDefinition(string tagName, int definitionId)
        {
            var ajaxResponse = new Dictionary<string, string>();
            ajaxResponse["actionType"] = "create";
            
            int tagId = AddTagToDataBase(tagName);
            var relation =
                db.DefTagRelations.FirstOrDefault(
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

                db.DefTagRelations.AddObject(relation);
                db.SaveChanges(); 
            }

            ajaxResponse["id"] = tagId.ToString(CultureInfo.InvariantCulture);
            ajaxResponse["message"] = "Тэг успешно добавлен";
            ajaxResponse["success"] = "true";
            ajaxResponse["body"] = tagName;
            return Json(ajaxResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpParamAction]
        public JsonResult RemoveFromDefinition(int tagId, int definitionId)
        {
            var ajaxResponse = new Dictionary<string, string>();
            ajaxResponse["actionType"] = "delete";

            var allRelationsWithTag = db.DefTagRelations.Where(x => x.TagID == tagId);
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

        private int AddTagToDataBase(string tagName)
        {
            var tag = db.Tags.FirstOrDefault(x => x.Name == tagName);
            if (tag == null)
            {
                tag = new Tag {Name = tagName};
                db.Tags.AddObject(tag);
                db.SaveChanges();
            }
            return tag.ID;
        }

        private void RemoveDefFromDataBase(DefTagRelation relation)
        {
            db.DefTagRelations.DeleteObject(relation);
            db.SaveChanges();
        }

        private void RemoveTagFromDataBase(int tagId)
        {
            var tag = db.Tags.FirstOrDefault(x => x.ID == tagId);
            db.Tags.DeleteObject(tag);
            db.SaveChanges();
        }

        /// <summary>
        /// Returns list of tag names contains mask 
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public JsonResult GetTagList(string mask)
        {
            var ajaxResponse =
                db.Tags.Where(x => x.Name.Contains(mask)).Select(x => x.Name).ToList();
            return Json(ajaxResponse, JsonRequestBehavior.AllowGet);
        }


        /*//
        // GET: /Tag/

        public ViewResult Index()
        {
            return View(db.Tags.ToList());
        }

        //
        // GET: /Tag/Details/5

        public ViewResult Details(int id)
        {
            Tag tag = db.Tags.Single(t => t.ID == id);
            return View(tag);
        }

        //
        // GET: /Tag/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Tag/Create

        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.AddObject(tag);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(tag);
        }
        
        //
        // GET: /Tag/Edit/5
 
        public ActionResult Edit(int id)
        {
            Tag tag = db.Tags.Single(t => t.ID == id);
            return View(tag);
        }

        //
        // POST: /Tag/Edit/5

        [HttpPost]
        public ActionResult Edit(Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Attach(tag);
                db.ObjectStateManager.ChangeObjectState(tag, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        //
        // GET: /Tag/Delete/5
 
        public ActionResult Delete(int id)
        {
            Tag tag = db.Tags.Single(t => t.ID == id);
            return View(tag);
        }

        //
        // POST: /Tag/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Tag tag = db.Tags.Single(t => t.ID == id);
            db.Tags.DeleteObject(tag);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
           base.Dispose(disposing);
        }*/
    }
}