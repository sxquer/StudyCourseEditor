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
        /// This method delivers test data to users 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            int sessionDBId = 0;
            TestData testData = GetTestData(out sessionDBId);
            if (testData == null) return RedirectToAction("End");

            ViewBag.QuestionToken = testData.GetQuestionHash();
            ViewBag.TestData = testData;

            return View(TemplateManager.Generate(testData.CurrentQuestionId, testData.TestSeed));
        }

        /// <summary>
        /// Action redirects to Index, if nothing to process
        /// </summary>
        /// <returns></returns>
        public ActionResult Process()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Process all test data
        /// </summary>
        /// <param name="collection">Form data from previous test page</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Process(FormCollection collection)
        {
            // Geting session information
            int sessionId = 0;
            TestData testData = GetTestData(out sessionId);


            // If test data is bad ending survey
            if (testData == null) return RedirectToAction("End");

            // If bad question token (for example student are trying to answer question previously cached in browser) redirecting to Index (will show last generated question)
            if (collection["QuestionToken"] != testData.GetQuestionHash())
                return RedirectToAction("Index");

            // Get previous question to check asnwer is correct
            GeneratedQuestion question =
                TemplateManager.Generate(testData.CurrentQuestionId,
                                         testData.TestSeed);

            // Remembering some nessasary data
            testData.ItemsTaken++;
            testData.TotalDifficultiesUsed += testData.CurrentQuestionDifficulty;

            // TODO: Add some behaviors. For the next programmers' generation. For example different options for different test types.
            // If amount of taken question equals amount of total questions in test, mark test as completed.
            if (testData.ItemsTaken >= testData.MaxAmountOfQuestions)
            {
                testData.TestCompleted = true;
            }

            // Calculating the amount that will change the difficulty
            double difficultyShift = 0.2 + (float) 2 / testData.ItemsTaken;

            // Checking answer is correct
            bool answerIsCorrect = CheckAnswerIsCorrect(question, collection["Answers"]);
            
            // Saving to question information about its own statistic 
            QuestionController.AddAttempt(testData.CurrentQuestionId, answerIsCorrect);

            // Adding data to 'ResultGraph'
            testData.AddPointToResultGraph(answerIsCorrect);
            // TODO: Temperory method for statistics. (Re)move
            testData.AddPointToRDF();
            
            // Correcting difficulty
            if (answerIsCorrect)
            {
                testData.TrueDifficultyLevel += difficultyShift;
                if (testData.TrueDifficultyLevel > 10)
                    testData.TrueDifficultyLevel = 10;
                
                testData.RightAnswersCount++;
            }
            else
            {
                testData.TrueDifficultyLevel -= difficultyShift;
                if (testData.TrueDifficultyLevel < 1)
                    testData.TrueDifficultyLevel = 1;
            }

            

            // TODO: Make CONST optional. For the next programmers' generation
            // Checking if measurement error lower than CONST
            if (testData.CalculateError() <= 0.5) testData.TestCompleted = true;

            // Selecting next question
            Question selectedQuestion = GetQuestion(testData);

            // Generates random seed, which helps to generate the same question from tamplate, if needed
            testData.TestSeed = TemplateManager.GetRandomSeed();

            // Remembering next question parameters
            testData.CurrentQuestionDifficulty = selectedQuestion.Difficulty;
            testData.CurrentQuestionId = selectedQuestion.ID;

            // Saving test session
            SetTestData(testData, sessionId);

            // Continue test or end it depending on TestCompleted flag
            return (testData.TestCompleted) ? RedirectToAction("End") : RedirectToAction("Index");
        }

        /// <summary>
        /// Initialize test
        /// </summary>
        /// <param name="subjectIds">Subject ids in which questions taking</param>
        /// <param name="testType">Test type</param>
        /// <param name="sourceType">See TestSourceTypes class</param>
        /// <param name="source">Nullable id of source</param>
        /// <returns></returns>
        private ActionResult StartTest(IEnumerable<int> subjectIds, TestType testType, int sourceType, int? source)
        {
            if (subjectIds == null || !subjectIds.Any()) return RedirectToAction("Index", "Home");

            var questionBank = new QuestionBank();
            var questions =
                _db.Questions.Where(
                    x => subjectIds.Contains(x.SubjectID) && x.IsPublished && x.Answers.Any());

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
        /// Wrap around StartTest for courses
        /// </summary>
        /// <param name="courseId">Course id</param>
        /// <param name="testType">Test type</param>
        /// <returns></returns>
        public ActionResult StartCourseTest(int courseId, TestType testType)
        {
            return StartTest(CourseController.GetById(courseId).Subjects.Select(x => x.ID), testType, TestSourceTypes.COURSE, courseId);
        }

        /// <summary>
        /// Wrap around StartTest for subject
        /// </summary>
        /// <param name="subjectId">Subject id</param>
        /// <param name="testType">Test type</param>
        /// <returns></returns>
        public ActionResult StartSubjectTest(int subjectId, TestType testType)
        {
            return StartTest(new List<int> { subjectId }, testType, TestSourceTypes.SUBJECT, subjectId);
        }


        /// <summary>
        /// Final test page
        /// </summary>
        /// <returns></returns>
        public ActionResult End()
        {
            int sessionDBId;
            TestData testData = GetTestData(out sessionDBId);
            ViewBag.ResultSaved = false;
            ViewBag.TestCompleted = false;

            ViewBag.TestData = testData;

            ClearTestData();
            if (testData == null) return View();
            

            var score = testData.CalculateMeasure();
            ViewBag.Score = score;
            ViewBag.TestCompleted = testData.TestCompleted;

            if (!testData.TestCompleted) return View();
            

            var user = Membership.GetUser();
            if (user != null && user.ProviderUserKey != null)
            {
                var result = new Result
                {
                    Source = testData.Source,
                    SourceType = testData.SourceType,
                    Date = TimeManager.GetCurrentTime(),
                    UserId = (Guid)user.ProviderUserKey,
                    Measure = score,
                    ResultGraph = testData.ResultGraph,
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
        /// <param name="userAnswer">String representation of user answer</param>
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

        /// <summary>
        /// Returns next question
        /// </summary>
        /// <param name="testData">Test session info</param>
        /// <returns></returns>
        private Question GetQuestion(TestData testData)
        {
            int[] priorityArray = GetDifficultyPriorityArray(testData.TrueDifficultyLevel);

            int i = 0;
            int nextQuestionId = -1;
            while (nextQuestionId == -1 && i < 10)
            {
                nextQuestionId = testData.QuestionBank.GetQuestion(priorityArray[i]);
                if (++i >= 10) throw new Exception("Нет подходящих вопросов");
            }

            return QuestionController.GetById(nextQuestionId);
        }

        /// <summary>
        /// Builds array of preferred difficulties for next question from best to worse
        /// </summary>
        /// <param name="realDifficulty">Calculated optimal difficulty for next question</param>
        /// <returns></returns>
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

        /// <summary>
        /// Saves test data
        /// </summary>
        /// <param name="data">Session data</param>
        /// <param name="sessionId">Session id (needed if data stores in database)</param>
        /// <param name="saveToCookie">'True' to make data stored in cookie</param>
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
        /// Returns test data
        /// </summary>
        /// <param name="databaseRecordId">Returns ID for records are storing in Data Base</param>
        /// <param name="getFromCookie">'True' to get data from cookie rather then database</param>
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


        /// <summary>
        /// Clears session data
        /// </summary>
        /// <param name="clearDataBaseRecord">True to clean data from db</param>
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
                catch { }
                
                testInfo.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(testInfo);
            }
            if (securityToken != null)
            {
                securityToken.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(securityToken);
            }

            
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult Error(string message)
        {
            ViewBag.Message = message;


            return View("Error");
        }
    }
}

