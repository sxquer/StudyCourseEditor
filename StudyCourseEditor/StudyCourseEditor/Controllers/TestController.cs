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
        private readonly Entities _db = new Entities(); 

        /// <summary>
        /// Отображает тест для пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var testData = GetTestData();
            if (testData == null) return RedirectToAction("End");

            ViewBag.QuestionToken = testData.GetQuestionHash();

            return View(TemplateManager.Generate(testData.CurrentQuestionId, testData.TestSeed));
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
            var testData = GetTestData();

            //If bad data then finish survey
            if (testData == null) return RedirectToAction("End");
            
            //If bad question token (for example student are trying to answer question previously cached in browser) redirect to last question
            if (collection["QuestionToken"] != testData.GetQuestionHash())
                return RedirectToAction("Index");

            var question = TemplateManager.Generate(testData.CurrentQuestionId, testData.TestSeed);

            testData.ItemsTaken++;
            testData.TotalDifficultiesUsed += testData.CurrentQuestionDifficulty;
            
            var difficultyShift = (int) Math.Round((float) 2 / testData.ItemsTaken);

            bool answerIsCorrect = CheckAnswerIsCorrect(question, collection["Answers"]);

            testData.AddPointToResultGraph(answerIsCorrect);

            //Корректируем сложность следующего вопроса
            if (answerIsCorrect)
            {
                testData.RightAnswersCount++;
                testData.CurrentQuestionDifficulty += difficultyShift;
            }
            else
            {
                testData.CurrentQuestionDifficulty -= difficultyShift;
            }

            var questionList = new List<Question>();
            int i = 0;
            while (questionList.Count == 0)
            {
                questionList = _db.Questions.Where(x => Math.Abs(x.Difficulty - testData.CurrentQuestionDifficulty) == i && (testData.SubjectsIds.Contains(x.SubjectID))).ToList();
                if (++i > testData.CurrentQuestionDifficulty) throw new Exception("Нет подходящих вопросов");
            }

            var selectedQuestion = questionList.ToArray()[new Random().Next(questionList.Count - 1)];

            testData.TestSeed = TemplateManager.GetRandomSeed();
            testData.CurrentQuestionDifficulty = selectedQuestion.Difficulty;
            testData.CurrentQuestionId = selectedQuestion.ID;

            SetTestData(testData);

            if (testData.CalculateError() < 0.3)
                return RedirectToAction("End");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Обработка результатов тестирования
        /// </summary>
        /// <returns></returns>
        public ActionResult End()
        {
            //TODO: Check if data == null
            var data = GetTestData();
            ClearTestData();
            if (data == null) return View();
            double score = data.CalculateMeasure();
            return View();
        }

        /// <summary>
        /// Validate user Answer
        /// </summary>
        /// <param name="question">Question that user answered</param>
        /// <param name="userAnswer"></param>
        /// <returns></returns>
        private bool CheckAnswerIsCorrect(GeneratedQuestion question, string userAnswer)
        {
            bool result = false;
            if (question.Type == QuestionType.SINGLE_CHOOSE_QUESTION)
            {
                int rightAnswer = int.Parse(userAnswer);
                if (question.Answers[rightAnswer].IsCorrect) result = true;
            }
            return result;
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

        private void ClearTestData()
        {
            var testInfo = Request.Cookies["TestInfo"];
            var securityToken = Request.Cookies["SecurityToken"];

            if (testInfo != null)
            {
                testInfo.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(testInfo);
            }
            if (securityToken != null)
            {
                securityToken.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(securityToken);
            }
        }



        public ActionResult Demo()
        {
            var ans = new List<GeneratedAnswer>
                          {
                              new GeneratedAnswer
                                  {
                                      Body = "First",
                                      IsCorrect = false,
                                  },
                                  new GeneratedAnswer
                                  {
                                      Body = "Second",
                                      IsCorrect = false,
                                  },
                                  new GeneratedAnswer
                                  {
                                      Body = "Third",
                                      IsCorrect = true,
                                  },
                                  new GeneratedAnswer
                                  {
                                      Body = "Fourth",
                                      IsCorrect = false,
                                  }
                          };

            var test = new GeneratedQuestion
            {
                Body = "Select Third Please",
                Type = QuestionType.SINGLE_CHOOSE_QUESTION,
                Answers = ans

            };
            ViewBag.QuestionToken = "140";
            return View("Index", test);
        }
    }
}
