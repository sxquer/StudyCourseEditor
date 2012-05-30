using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StudyCourseEditor.Models;
using StudyCourseEditor.TestClasses;
using StudyCourseEditor.Tools;
using Question = StudyCourseEditor.Models.Question;

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
            int sessionDBId = 0;
            TestData testData = GetTestData(out sessionDBId);
            if (testData == null) return RedirectToAction("End");

            ViewBag.QuestionToken = testData.GetQuestionHash();
            ViewBag.TestData = testData;

            return View(TemplateManager.Generate(testData.CurrentQuestionId,
                                              testData.TestSeed));
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
            int sessionId = 0;
            TestData testData = GetTestData(out sessionId);


            //If bad data then finish survey
            if (testData == null) return RedirectToAction("End");

            //If bad question token (for example student are trying to answer question previously cached in browser) redirect to last question
            if (collection["QuestionToken"] != testData.GetQuestionHash())
                return RedirectToAction("Index");

            GeneratedQuestion question =
                TemplateManager.Generate(testData.CurrentQuestionId,
                                         testData.TestSeed);

            testData.ItemsTaken++;
            testData.TotalDifficultiesUsed += testData.CurrentQuestionDifficulty;

            if (testData.ItemsTaken > testData.MaxAmountOfQuestions)
            {
                testData.TestCompleted = true;
            }

            double difficultyShift = 0.2 + (float) 2 / testData.ItemsTaken;

            bool answerIsCorrect = CheckAnswerIsCorrect(question, collection["Answers"]);
            QuestionController.AddAttempt(testData.CurrentQuestionId, answerIsCorrect);

            testData.AddPointToResultGraph(answerIsCorrect);

            //Корректируем сложность следующего вопроса
            if (answerIsCorrect)
            {
                testData.RightAnswersCount++;
                testData.TrueDifficultyLevel += difficultyShift;
            }
            else
            {
                testData.TrueDifficultyLevel -= difficultyShift;
            }

            if (testData.CalculateError() < 0.3)
                testData.TestCompleted = true;

            Question selectedQuestion = GetQuestion(testData);

            testData.TestSeed = TemplateManager.GetRandomSeed();
            testData.CurrentQuestionDifficulty = selectedQuestion.Difficulty;
            testData.CurrentQuestionId = selectedQuestion.ID;

            SetTestData(testData, sessionId);

            return (testData.TestCompleted) ? RedirectToAction("End") : RedirectToAction("Index");
        }

        /// <summary>
        /// Стартовая страница для теста
        /// </summary>
        /// <returns></returns>
        private ActionResult StartTest(IEnumerable<int> subjectIds, TestType testType, int sourceType, int? source)
        {
            if (subjectIds == null || !subjectIds.Any()) return RedirectToAction("Index", "Home");

            var questionBank = new QuestionBank();
            var questions =
                _db.Questions.Where(
                    x => subjectIds.Contains(x.SubjectID) && x.IsPublished);

            if (!questions.Any()) return RedirectToAction("Error", new {message = "Для теста не найдено ни одного вопроса. Обратитесь к администратору."});

            for (int i = 1; i <= 10; i++)
            {
               questionBank.Questions.Add(new List<int>(questions.Where(x => x.Difficulty == i).Select(x => x.ID))); 
            }
            
            var testData = new TestData
                               {
                                   TestCompleted = false,
                                   TrueDifficultyLevel = 5,
                                   SubjectsIds = subjectIds.ToList(),
                                   QuestionBank = questionBank,
                                   Started = TimeManager.GetCurrentTime(),
                                   TestType = testType,
                                   MaxAmountOfQuestions = _db.Questions.Count(x => subjectIds.Contains(x.SubjectID) && x.IsPublished),
                                   SourceType = sourceType,
                                   Source = source,
                               };

            Question selectedQuestion = GetQuestion(testData);

            testData.TestSeed = TemplateManager.GetRandomSeed();
            testData.CurrentQuestionDifficulty = selectedQuestion.Difficulty;
            testData.CurrentQuestionId = selectedQuestion.ID;


            SetTestData(testData);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Стартовая страница для теста
        /// </summary>
        /// <returns></returns>
        public ActionResult StartCourseTest(int courseId, TestType testType)
        {
            return StartTest(CourseController.GetById(courseId).Subjects.Select(x => x.ID), testType, TestSourceTypes.COURSE, courseId);
        }

        /// <summary>
        /// Стартовая страница для теста
        /// </summary>
        /// <returns></returns>
        public ActionResult StartSubjectTest(int subjectId, TestType testType)
        {
            return StartTest(new List<int> { subjectId }, testType, TestSourceTypes.SUBJECT, subjectId);
        }


        /// <summary>
        /// Обработка результатов тестирования
        /// </summary>
        /// <returns></returns>
        public ActionResult End()
        {
            int sessionDBId;
            TestData data = GetTestData(out sessionDBId);
            ViewBag.ResultSaved = false;
            ViewBag.TestCompleted = false;
            
            ClearTestData();
            if (data == null) return View();
            

            var score = data.CalculateMeasure();
            ViewBag.Score = score;
            ViewBag.TestCompleted = data.TestCompleted;

            if (!data.TestCompleted) return View();
            

            var user = Membership.GetUser();
            if (user != null && user.ProviderUserKey != null)
            {
                var result = new Result
                {
                    Source = data.Source,
                    SourceType = data.SourceType,
                    Date = TimeManager.GetCurrentTime(),
                    UserId = (Guid)user.ProviderUserKey,
                    Measure = score,
                    ResultGraph = data.ResultGraph,
                };
                ResultController.Add(result);
                ViewBag.ResultSaved = true;
            }
            
            return View();
        }

        /// <summary>
        /// Validate user Answer
        /// </summary>
        /// <param name="question">Question that user answered</param>
        /// <param name="userAnswer"></param>
        /// <returns></returns>
        private bool CheckAnswerIsCorrect(GeneratedQuestion question,
                                          string userAnswer)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(userAnswer)) return false;

            if (question.QuestionType == 1)
            {
                int rightAnswer = int.Parse(userAnswer);
                if (question.Answers[rightAnswer].IsCorrect) result = true;
            }

            if (question.QuestionType == 2)
            {
                result = true;
                var answerArray = userAnswer.Split(',');
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    if (question.Answers[i].IsCorrect != answerArray.Contains(i.ToString()))
                        result = false;
                }
            }

            if (question.QuestionType == 3)
            {
                foreach (var ans in question.Answers)
                {
                    if (ans.Body == userAnswer) result = true;
                }
            }

            return result;
        }

        private Question GetQuestion(TestData testData)
        {
            var questionList = new List<Question>();

            int[] priorityArray =
                GetDifficultyPriorityArray(testData.TrueDifficultyLevel);

            int i = 0;
            while (questionList.Count == 0 && i < 10)
            {
                int dif = priorityArray[i];
                questionList =
                    _db.Questions.Where(
                        x =>
                        x.Difficulty == dif &&
                        (testData.SubjectsIds.Contains(x.SubjectID))).ToList
                        ();
                if (++i >= 10) throw new Exception("Нет подходящих вопросов");
            }

            return questionList.FirstOrDefault();
        }

        private int[] GetDifficultyPriorityArray(double realDifficulty)
        {
            var result = new int[10];

            var currentValue = (int) Math.Round(realDifficulty);
            int inverter = (realDifficulty / currentValue > 1) ? 1 : -1;
            int filledCount = 10;

            for (int i = 0; i < 10; i++)
            {
                result[i] = currentValue;
                currentValue = currentValue + (i + 1) * inverter;
                inverter = -inverter;
                if (currentValue > 10 || currentValue <= 0)
                {
                    filledCount = i + 1;
                    break;
                }
            }

            while (filledCount < 10)
            {
                result[filledCount] = result[filledCount - 1] + inverter;
                filledCount++;
            }

            return result;
        }

        private void SetTestData(TestData data, int sessionId = 0, bool saveToCookie = false)
        {
            
            string dataString = XmlManager.SerializeObjectUTF8(data);
            
            string tokenString;
            string testInfoString;

            if (saveToCookie)
            {
                testInfoString = HttpUtility.UrlEncode(dataString);
                tokenString = MD5HashManager.GenerateKey(testInfoString);
            }
            else
            {
                tokenString = MD5HashManager.GenerateKey(dataString);
                testInfoString = TestSessionManager.Write(dataString, sessionId).ToString(CultureInfo.InvariantCulture);
            }


            var testInfo = new HttpCookie("TestInfo", testInfoString);
            var securityToken = new HttpCookie("SecurityToken", tokenString);

            Response.Cookies.Add(testInfo);
            Response.Cookies.Add(securityToken);

            TempData["testInfo"] = testInfo;
            TempData["securityToken"] = securityToken;

        }

        /// <summary>
        /// ПReturn test data
        /// </summary>
        /// <param name="databaseRecordId">Returns ID for records are storing in Data Base</param>
        /// <param name="getFromCookie">Where data're storing?</param>
        /// <returns></returns>
        private TestData GetTestData(out int databaseRecordId, bool getFromCookie = false)
        {
            var testInfoCookie = (HttpCookie)TempData["TestInfo"];
            var securityTokenCookie = (HttpCookie)TempData["SecurityToken"];

            databaseRecordId = 0;

            if (testInfoCookie == null || securityTokenCookie == null || string.IsNullOrWhiteSpace(testInfoCookie.Value) || string.IsNullOrWhiteSpace(securityTokenCookie.Value))
            {
                testInfoCookie = Request.Cookies["TestInfo"];
                securityTokenCookie = Request.Cookies["SecurityToken"];

                if (testInfoCookie == null || securityTokenCookie == null ||
                    string.IsNullOrWhiteSpace(testInfoCookie.Value) ||
                    string.IsNullOrWhiteSpace(securityTokenCookie.Value)) return null;
            }

            else
            {
                Response.Cookies.Add(testInfoCookie);
                Response.Cookies.Add(securityTokenCookie);
            }


            string data = testInfoCookie.Value;
            string securityToken = securityTokenCookie.Value;
                
            if (getFromCookie)
            {
                data = HttpUtility.UrlDecode(data);
            }
            else
            {
                var id = 0;
                try
                {
                    id = int.Parse(testInfoCookie.Value);
                }
                catch (Exception)
                {
                    return null;
                }

                databaseRecordId = id;
                data = TestSessionManager.Read(id);
            }
            

            if (MD5HashManager.GenerateKey(data) != securityToken) return null;

            return XmlManager.DeserializeObject<TestData>(data);
        }

        private void ClearTestData(bool clearDataBaseRecord = true)
        {
            HttpCookie testInfo = Request.Cookies["TestInfo"];
            HttpCookie securityToken = Request.Cookies["SecurityToken"];

            if (testInfo != null)
            {
                try
                {
                    int id = int.Parse(testInfo.Value);
                    TestSessionManager.Remove(id);

                }
                catch (Exception ex) { }
                
                testInfo.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(testInfo);
            }
            if (securityToken != null)
            {
                securityToken.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(securityToken);
            }

            
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View("Error");
        }
    }
}

