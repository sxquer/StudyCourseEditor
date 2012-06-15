using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Serialization;
using StudyCourseEditor.Tools;

namespace StudyCourseEditor.TestClasses
{
    [Serializable]
    [XmlRoot("TD")]
    public class TestData
    {
        /// <summary>
        /// Id of all subjects using in current test
        /// </summary>
        [XmlElement(ElementName = "SI")]
        public List<int> SubjectsIds { get; set; }

        /// <summary>
        /// All allowed questions
        /// </summary>
        public QuestionBank QuestionBank { get; set; }

        /// <summary>
        /// True if test should be finished
        /// </summary>
        public bool TestCompleted { get; set; }
        
        /// <summary>
        /// Difficulty level of last question
        /// </summary>
        [XmlElement(ElementName = "CQD")]
        public int CurrentQuestionDifficulty { get; set; }

        /// <summary>
        /// Accurate difficulty level
        /// </summary>
        [XmlElement(ElementName = "TDL")]
        public double TrueDifficultyLevel { get; set; }

        /// <summary>
        /// Last question id
        /// </summary>
        [XmlElement(ElementName = "CQI")]
        public int CurrentQuestionId { get; set; }

        /// <summary>
        /// Unique seed for TemplateManager
        /// </summary>
        [XmlElement(ElementName = "TS")]
        public int TestSeed { get; set; }

        /// <summary>
        /// Amount of answered question
        /// </summary>
        [XmlElement(ElementName = "IT")]
        public int ItemsTaken { get; set; }

        /// <summary>
        /// Sum of answered question's difficulties
        /// </summary>
        [XmlElement(ElementName = "TDU")]
        public int TotalDifficultiesUsed { get; set; }

        /// <summary>
        /// Amount of correct answered questions
        /// </summary>
        [XmlElement(ElementName = "RAC")]
        public int RightAnswersCount { get; set; }

        /// <summary>
        /// DateTime when test started
        /// </summary>
        [XmlElement(ElementName = "S")]
        public DateTime Started { get; set; }

        /// <summary>
        /// Type of test
        /// </summary>
        [XmlElement(ElementName = "TT")]
        public TestType TestType { get; set; }

        /// <summary>
        /// Maximum number of question
        /// </summary>
        [XmlElement(ElementName = "MAOQ")]
        public int MaxAmountOfQuestions { get; set; }

        /// <summary>
        /// Link to test source. Course / Subjects / Null for manually generated test
        /// </summary>
        [XmlElement(ElementName = "L")]
        public int? Source { get; set; }


        /// <summary>
        /// See the 'TestSourseType' class
        /// </summary>
        [XmlElement(ElementName = "LT")]
        public int SourceType { get; set; }

        /// <summary>
        /// Data for graphic presentation of test's results. Format "difficulty_isCorrect;"
        /// </summary>
        [XmlElement(ElementName = "RG")]
        public string ResultGraph { get; set; }

        /// <summary>
        /// Temporary data. RealDifficultyGraph - RDF
        /// </summary>
        [XmlElement(ElementName = "RDF")]
        public string RDF { get; set; }

        /// <summary>
        /// Returns question hash
        /// </summary>
        /// <returns></returns>
        public string GetQuestionHash()
        {
            return MD5HashManager.GenerateKey(CurrentQuestionId + TestSeed);
        }


        /// <summary>
        /// Reaturns measurement error
        /// </summary>
        /// <returns></returns>
        public double CalculateError()
        {
            float wrongAnswersCount = ItemsTaken - RightAnswersCount;
            float rightAnswersCount = RightAnswersCount;

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

            return
                Math.Sqrt(ItemsTaken / (wrongAnswersCount * rightAnswersCount));
        }

        /// <summary>
        /// Returns assessment of knowledge level
        /// </summary>
        /// <returns></returns>
        public double CalculateMeasure()
        {
            float wrongAnswersCount = ItemsTaken - RightAnswersCount;
            float rightAnswersCount = RightAnswersCount;

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

            return ((float) TotalDifficultiesUsed / ItemsTaken) +
                   Math.Log(rightAnswersCount / wrongAnswersCount);
        }

        /// <summary>
        /// 	Add New Point to ResultGraph
        /// </summary>
        /// <param name = "answerIsCorrect"></param>
        public void AddPointToResultGraph(bool answerIsCorrect)
        {
            ResultGraph += CurrentQuestionDifficulty + "_" +
                           ((answerIsCorrect) ? 1 : 0) + ";";
        }

        public void AddPointToRDF()
        {
            RDF += Math.Round(TrueDifficultyLevel, 1).ToString(CultureInfo.InvariantCulture).Replace(",", ".") + ";";
        }
    }
}