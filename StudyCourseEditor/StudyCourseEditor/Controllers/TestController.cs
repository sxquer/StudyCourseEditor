using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{
    public class TestController : Controller
    {
        readonly StudyCourseDB _db = new StudyCourseDB(); 

        public ActionResult CreateDummyData()
        {
            var course = new Course
                             {
                                 Name = "Математика",
                                 Description = "Математика - царица наук",
                             };
            _db.Courses.Add(course);
            _db.SaveChanges();


            _db.Subjects.Add(new Subject
                              {
                                  Name = "Целые числа",
                                  CourseID = course.ID,
                              });

            _db.SaveChanges();

            _db.Subjects.Add(new Subject
                                {
                                    Name = "Дробные числа",
                                    CourseID = course.ID,
                                });
            _db.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }

    }
}
