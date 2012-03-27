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

            if (CalculateError(data) < 0.3)
                return RedirectToAction("FinishExam", data);

            return View();
        }

        /// <summary>
        /// Обработка результатов тестирования
        /// </summary>
        /// <returns></returns>
        public ActionResult FinishExam(TestData data)
        {
            double score = CalculateMeasure(data);
            return View();
        }

        /// <summary>
        /// Возвращает примерную оценку уровня знаний студента.
        /// </summary>
        /// <param name="difficultiesUsed">Сумма сложностей всех заданных вопросов</param>
        /// <param name="itemsTaken">Количество пройденных заданий</param>
        /// <param name="rightAnswersCount">Количество верных ответов</param>
        /// <returns>Оценка уровня знаний</returns>
        private double CalculateMeasure(float difficultiesUsed, float itemsTaken, float rightAnswersCount)
        {
            float wrongAnswersCount = itemsTaken - rightAnswersCount;

            if (Math.Abs(wrongAnswersCount - 0) < 0.001)
            {
                wrongAnswersCount += 0.5f;
                rightAnswersCount -= 0.5f;
            }

            if (Math.Abs(rightAnswersCount - 0) < 0.001)
            {
                wrongAnswersCount -= 0.5f;
                rightAnswersCount += 0.5f;
            }

            return (difficultiesUsed / itemsTaken) + Math.Log(rightAnswersCount / wrongAnswersCount);
        }

        /// <summary>
        /// Возвращает примерную оценку уровня знаний студента.
        /// </summary>
        /// <param name="data">Набо данных о тесте</param>
        /// <returns>Оценка уровня знаний</returns>
        private double CalculateMeasure(TestData data)
        {
            return CalculateMeasure(data.TotalDifficultiesUsed, data.ItemsTaken, data.RightAnswersCount);
        }

        /// <summary>
        /// Возвращает текущую ошибку измерения. Необходима для критерия остановки.
        /// </summary>
        /// <param name="itemsTaken">Количество пройденных заданий</param>
        /// <param name="rightAnswersCount">Количество верных ответов</param>
        /// <returns>Ошбика</returns>
        private double CalculateError(float itemsTaken, float rightAnswersCount)
        {
            float wrongAnswersCount = itemsTaken - rightAnswersCount;
            
            if (Math.Abs(wrongAnswersCount - 0) < 0.001)
            {
                wrongAnswersCount += 0.5f;
                rightAnswersCount -= 0.5f;
            }

            if (Math.Abs(rightAnswersCount - 0) < 0.001)
            {
                wrongAnswersCount -= 0.5f;
                rightAnswersCount += 0.5f;
            }

            return Math.Sqrt(itemsTaken / (wrongAnswersCount * rightAnswersCount));
        }


        /// <summary>
        /// Возвращает текущую ошибку измерения. Необходима для критерия остановки.
        /// </summary>
        /// <param name="data">Набо данных о тесте</param>
        /// <returns>Ошбика</returns>
        private double CalculateError(TestData data)
        {
            return CalculateError(data.ItemsTaken, data.RightAnswersCount);
        }
    }
}
