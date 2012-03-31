using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;
using StudyCourseEditor.Tools;

namespace StudyCourseEditor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Под конструкцией.";


            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult TestProcess(string id)
        {
            var ans = new List<Answer>
                          {
                              new Answer
                                  {
                                      Body = "232",
                                      IsCorrect = false,
                                  },
                                  new Answer
                                  {
                                      Body = "232",
                                      IsCorrect = false,
                                  },
                                  new Answer
                                  {
                                      Body = "232",
                                      IsCorrect = false,
                                  },
                                  new Answer
                                  {
                                      Body = "232",
                                      IsCorrect = false,
                                  }
                          };

            var test = new GeneratedTest
                           {
                               Body = "lol3213",
                               Type = QuestionType.MULTI_CHOOSE_QUESTION,
                               Answers = ans
                            
                           };

            var lol = new TestData
                          {
                            CurrentQuestionDifficulty = 54,
                            CurrentTest = test,
                            ItemsTaken = 24,
                            ResultGraph = "19;03;19;16;05;19;03;19;16;05;19;03;19;16;05;19;03;19;16;05;",
                            RightAnswersCount = 16,
                            Started = DateTime.Parse("00:00:00"),
                            TotalDifficultiesUsed = 38
                          };

            string xml = XmlManager.SerializeObjectUTF8(lol);
            string aesCode = CryptoAesManager.EncryptStringAES(xml, "Dadada");
            string hash = MD5HashManager.GenerateKey(aesCode);
            ViewBag.Xml = xml;
            ViewBag.XmlLength = xml.Length;
            ViewBag.Aes = aesCode;
            ViewBag.AesLength = aesCode.Length;
            ViewBag.Hash = hash;

            Response.Cookies.Add(new HttpCookie("SecurityToken", hash));
/*
            for (int i = 0; i < 2; i++)
            {
                aesCode += aesCode;
            }
            */
             Response.Cookies.Add(new HttpCookie("DataToken", aesCode));
   
            

            var some = XmlManager.DeserializeObject<TestData>(xml);
            return View("TestShow");
            return RedirectToAction("TestShow");
        }

        public ActionResult TestShow()
        {
            return View();
        }

    }
}
