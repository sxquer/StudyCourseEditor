using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using StudyCourseEditor.Tools;

namespace StudyCourseEditor.Models
{
    [NotMapped]
    [Serializable]
    [XmlRoot("TD")]
    public class TestData
    {
        /// <summary>
        /// Id of all subjects using in current test
        /// </summary>
        [XmlElement(ElementName = "SI")]
        public List<int> SubjectsIds { get; set; }

        [XmlElement(ElementName = "CQD")]
        public int CurrentQuestionDifficulty { get; set; }
        
        [XmlElement(ElementName = "CQI")]
        public int CurrentQuestionId { get; set; }

        [XmlElement(ElementName = "TS")]
        public int TestSeed { get; set; }

        [XmlElement(ElementName = "IT")]
        public int ItemsTaken { get; set; }

        [XmlElement(ElementName = "TDU")]
        public int TotalDifficultiesUsed { get; set; }

        [XmlElement(ElementName = "RAC")]
        public int RightAnswersCount { get; set; }

        [XmlElement(ElementName = "S")]
        public DateTime Started { get; set; }

        /// <summary>
        /// Data for graphic presentation of test's results. Format "difficulty_isCorrect;"
        /// </summary>
        [XmlElement(ElementName = "RG")]
        public string ResultGraph { get; set; }

        /// <summary>
        /// Возвращает хэш от вопроса по ключу и сиду
        /// </summary>
        /// <returns></returns>
        public string GetQuestionHash()
        {
            return MD5HashManager.GenerateKey(CurrentQuestionId + TestSeed);
        }



        /// <summary>
        /// Возвращает текущую ошибку измерения. Необходима для критерия остановки.
        /// </summary>
        /// <returns>Ошибка</returns>
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

            return Math.Sqrt(ItemsTaken / (wrongAnswersCount * rightAnswersCount));
        }

        /// <summary>
        /// Возвращает примерную оценку уровня знаний студента.
        /// </summary>
        /// <returns>Оценка уровня знаний</returns>
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

            return ((float)TotalDifficultiesUsed / ItemsTaken) + Math.Log(rightAnswersCount / wrongAnswersCount);
        }

        /// <summary>
        /// Add New Point to ResultGraph
        /// </summary>
        /// <param name="answerIsCorrect"></param>
        public void AddPointToResultGraph(bool answerIsCorrect)
        {
            ResultGraph += CurrentQuestionDifficulty + "_" + ((answerIsCorrect) ? 1 : 0) + ";";
        }
        
    }

    /// <summary>
    /// Question from database
    /// </summary>
    public class Question
    {
        public int ID { get; set; }

        /// <summary>
        /// Question's template
        /// </summary>
        public string Body { get; set; }

        public int Difficulty { get; set; }

        /// <summary>
        /// List of possible answers to this question
        /// </summary>
        public ICollection<Answer> Answers { get; set; }

        [ForeignKey("Subject")]
        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
    }


    /// <summary>
    /// Answer from database
    /// </summary>
    public class Answer
    {
        public int ID { get; set; }

        /// <summary>
        /// Answer Template
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Checked if answer is correct
        /// </summary>
        public bool IsCorrect { get; set; }


        /// <summary>
        /// Link to question
        /// </summary>
        [ForeignKey("Question")]
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }
    }

    /// <summary>
    /// Fully generated test
    /// </summary>
    [NotMapped]
    public class GeneratedQuestion
    {
        /// <summary>
        /// Question's text
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// List of answers
        /// </summary>
        public List<GeneratedAnswer> Answers { get; set; }


        /// <summary>
        /// Type of question. 1 by Default
        /// </summary>
        public int Type { get; set; }
    }

    /// <summary>
    /// Fully generated answer
    /// </summary>
    [NotMapped]
    public class GeneratedAnswer
    {
        /// <summary>
        /// Answer's text
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Answer is correct
        /// </summary>
        public bool IsCorrect { get; set; }
    }

    /// <summary>
    /// Типы вопросов
    /// </summary>
    [NotMapped]
    public class QuestionType
    {
        /// <summary>
        /// Вопрос с одним верным вариантом ответа
        /// </summary>
        public const int SINGLE_CHOOSE_QUESTION = 1;

        /// <summary>
        /// Вопрос с несколькими вариантами правильных ответов
        /// </summary>
        public const int MULTI_CHOOSE_QUESTION = 2;

        /// <summary>
        /// Вопрос, требующий ручного ввода ответа
        /// </summary>
        public const int FREE_QUESTION = 3;
    }
}