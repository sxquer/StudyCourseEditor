using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;
using StudyCourseEditor.Tools;

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
            var testData = GetTestData();
            if (testData == null) return RedirectToAction("FinishExam");



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
            var data = GetTestData();

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

            SetTestData(data);

            if (data.CalculateError() < 0.3)
                return RedirectToAction("FinishExam", data);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Обработка результатов тестирования
        /// </summary>
        /// <returns></returns>
        public ActionResult FinishExam()
        {
            //TODO: Check if data == null
            var data = GetTestData();
            
            double score = data.CalculateMeasure();
            return View();
        }

        private void SetTestData(TestData data)
        {
            string cookieString = CryptoXorManager.Process(XmlManager.SerializeObjectUTF8(data), 17);

            var testInfo = new HttpCookie("TestInfo", cookieString);
            var securityToken = new HttpCookie("SecurityToken", MD5HashManager.GenerateKey(cookieString));

            Response.Cookies.Add(testInfo);
            Response.Cookies.Add(securityToken);

            Request.Cookies.Add(testInfo);
            Request.Cookies.Add(securityToken);
        }

        private TestData GetTestData()
        {
            var testInfo = Request.Cookies["TestInfo"];
            var securityToken = Request.Cookies["SecurityToken"];

            if (testInfo == null || securityToken == null) return null;
            if (MD5HashManager.GenerateKey(testInfo.Value) != securityToken.Value) return null;

            return XmlManager.DeserializeObject<TestData>(CryptoXorManager.Process(testInfo.Value, 17));
        }

    }
}
