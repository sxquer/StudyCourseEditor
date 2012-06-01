using System.Data;
using System.Linq;
using System.Web.Mvc;
using StudyCourseEditor.Extensions;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class DefinitionController : Controller
    {
        private readonly Entities _db = new Entities();

        //
        // GET: /Definition/

        public ViewResult Index()
        {
            return View(_db.Definitions.ToList());
        }

        //
        // GET: /Definition/Details/5

        public ViewResult Details(int id)
        {
            Definition definition = _db.Definitions.Single(d => d.ID == id);
            return View(definition);
        }

        //
        // GET: /Definition/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Definition/Create
        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateAndBack(Definition definition)
        {
            return Create(definition, "Index");
        }

        //
        // POST: /Definition/Create
        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateAndContinue(Definition definition)
        {
            return Create(definition, "Edit");
        }

        private ActionResult Create(Definition definition, string action)
        {
            if (_db.Definitions.FirstOrDefault(d => d.Name.ToLower() == definition.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Name","Полное имя должно быть уникальным. Похожее определение уже есть");
                return View("Create");
            }

            if (ModelState.IsValid)
            {
                _db.Definitions.AddObject(definition);
                _db.SaveChanges();
                return RedirectToAction(action, new { id = definition.ID });
            }

            return View("Create");
        }

        //
        // GET: /Definition/Edit/5

        public ActionResult Edit(int id)
        {
            Definition definition = _db.Definitions.Single(d => d.ID == id);

            if (!AccountController.CanUserAccess(definition)) 
            return RedirectToAction("AccessDenied", "Account",
                new { message = string.Format("У вас нет права редактировать определение {0} ({1}). Обратитесь к автору или администратору", 
                    definition.DisplayName, definition.Name)});

            var tagIds = definition.DefTagRelations.Select(x => x.TagID);
            ViewBag.Tags = _db.Tags.Where(x => tagIds.Contains(x.ID));
            return View(definition);
        }

        //
        // POST: /Definition/Edit/5

        [HttpPost]
        public ActionResult Edit(Definition definition)
        {

            if (!AccountController.CanUserAccess(definition))
                return RedirectToAction("AccessDenied", "Account",
                    new
                    {
                        message = string.Format("У вас нет права редактировать определение {0} ({1}). Обратитесь к автору или администратору",
                            definition.DisplayName, definition.Name)
                    });

            if (ModelState.IsValid)
            {
                _db.Definitions.Attach(definition);
                _db.ObjectStateManager.ChangeObjectState(definition, EntityState.Modified);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(definition);
        }

        //
        // GET: /Definition/Delete/5

        public ActionResult Delete(int id)
        {
            Definition definition = _db.Definitions.Single(d => d.ID == id);

            if (!AccountController.CanUserAccess(definition))
                return RedirectToAction("AccessDenied", "Account",
                    new
                    {
                        message = string.Format("У вас нет права удалять определение {0} ({1}). Обратитесь к автору или администратору",
                            definition.DisplayName, definition.Name)
                    });
            
            return View(definition);
        }

        //
        // POST: /Definition/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Definition definition = _db.Definitions.Single(d => d.ID == id);

            if (!AccountController.CanUserAccess(definition))
                return RedirectToAction("AccessDenied", "Account",
                    new
                    {
                        message = string.Format("У вас нет права удалять определение {0} ({1}). Обратитесь к автору или администратору",
                            definition.DisplayName, definition.Name)
                    });

            _db.Definitions.DeleteObject(definition);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        /// Remote validation for Definition Name
        /// </summary>
        /// <param name="name">String to valdate</param>
        /// <returns>Json representation</returns>
        public ActionResult CheckNameIsUnique(string name)
        {
            return Json(_db.Definitions.FirstOrDefault(d => d.Name.ToLower() == name.ToLower()) == null,
                        JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}