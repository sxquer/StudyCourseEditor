﻿using System;
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
            ViewBag.TestData = testData;


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
            
            var difficultyShift = 0.2 + (float) 2 / testData.ItemsTaken;

            bool answerIsCorrect = CheckAnswerIsCorrect(question, collection["Answers"]);

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
                return RedirectToAction("End");

            var selectedQuestion = GetQuestion(testData);

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
        public ActionResult StartTest(IEnumerable<int> subjectIds)
        {
            var testData = new TestData
                            {
                               TrueDifficultyLevel = 5,
                               SubjectsIds = subjectIds.ToList(),
                               Started = TimeManager.GetCurrentTime(),
                            };  

            var selectedQuestion = GetQuestion(testData);

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
        public ActionResult StartCourseTest(int courseId)
        {
            return StartTest(CourseController.GetById(courseId).Subjects.Select(x => x.ID));
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

            if (string.IsNullOrWhiteSpace(userAnswer)) return false;

            if (question.Type == QuestionType.SINGLE_CHOOSE_QUESTION)
            {
                int rightAnswer = int.Parse(userAnswer);
                if (question.Answers[rightAnswer].IsCorrect) result = true;
            }
            return result;
        }

        private Question GetQuestion(TestData testData)
        {
            var questionList = new List<Question>();

            var priorityArray = GetDifficultyPriorityArray(testData.TrueDifficultyLevel);

            int i = 0;
            int dif = 0;
            while (questionList.Count == 0 && i < 10)
            {
                dif = priorityArray[i];
                questionList = _db.Questions.Where(x => x.Difficulty == dif && (testData.SubjectsIds.Contains(x.SubjectID))).ToList();
                if (++i >= 10) throw new Exception("Нет подходящих вопросов");
            }

            return questionList.FirstOrDefault();
        }

        private int[] GetDifficultyPriorityArray(double realDifficulty)
        {
            var result = new int[10];
            
            var currentValue = (int) Math.Round(realDifficulty);
            int inverter = (realDifficulty / currentValue > 1) ? 1 : -1;
            var filledCount = 10;

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

        private void SetTestData(TestData data)
        {
            string cookieString = HttpUtility.UrlEncode(XmlManager.SerializeObjectUTF8(data));

            var testInfo = new HttpCookie("TestInfo", cookieString);
            var securityToken = new HttpCookie("SecurityToken", MD5HashManager.GenerateKey(cookieString));

            Response.Cookies.Add(testInfo);
            Response.Cookies.Add(securityToken);

            TempData["testInfo"] = testInfo;
            TempData["securityToken"] = securityToken;
        }

        private TestData GetTestData()
        {
            var testInfo = (HttpCookie)TempData["TestInfo"];
            var securityToken = (HttpCookie)TempData["SecurityToken"];

            if (testInfo == null || securityToken == null || string.IsNullOrWhiteSpace(testInfo.Value) || string.IsNullOrWhiteSpace(securityToken.Value))
            {
                testInfo = Request.Cookies["TestInfo"];
                securityToken = Request.Cookies["SecurityToken"];

                if (testInfo == null || securityToken == null || string.IsNullOrWhiteSpace(testInfo.Value) || string.IsNullOrWhiteSpace(securityToken.Value)) return null;
            }

            else
            {
                Response.Cookies.Add(testInfo);
                Response.Cookies.Add(securityToken);
            }
            
            if (MD5HashManager.GenerateKey(testInfo.Value) != securityToken.Value) return null;

            return XmlManager.DeserializeObject<TestData>(HttpUtility.UrlDecode(testInfo.Value));
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

