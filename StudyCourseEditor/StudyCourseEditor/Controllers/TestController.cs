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

        /// <summary>
        /// Отображает тест для пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Process()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Обрабатывает информацию о тесте и введенные ответы
        /// </summary>
        /// <param name="collection">Данные из формы</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Process(FormCollection collection)
        {
            var data = new TestData();

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

            return RedirectToAction("Index");
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

        private void SetTestData(TestData data)
        {
        }

        private TestData GetTestData()
        {
            return new TestData();
        }

    }
}
