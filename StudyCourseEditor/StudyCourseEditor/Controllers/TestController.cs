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


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TestData data)
        {
            //TODO: Получить реальное значение
            bool responseIsCorrect = true;

            data.ItemsTaken++;
            data.TotalDifficultiesUsed += data.CurrentQuestionDifficulty;

            //Корректируем сложность следующего вопроса
            if (responseIsCorrect)
            {
                data.RightAnswersCount++;
                data.CurrentQuestionDifficulty += 2 / data.ItemsTaken;
            }
            else
            {
                data.CurrentQuestionDifficulty -= 2 / data.ItemsTaken;
            }

            if (data.CalculateError() < 0.3)
                return RedirectToAction("FinishExam", data);

            return View(data.NextQuestion());
        }

        /// <summary>
        /// Обработка результатов тестирования
        /// </summary>
        /// <returns></returns>
        public ActionResult FinishExam(TestData data)
        {
            double score = data.CalculateMeasure();
            return View();
        }

    }
}
