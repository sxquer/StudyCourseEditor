﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using StudyCourseEditor.Tools;

namespace StudyCourseEditor.Models
{
    public class Question
    {
        public int ID { get; set; }
        public string Body { get; set; }
        public int Difficulty { get; set; }

        public ICollection<Question> Questions { get; set; } 
    }

    [NotMapped]
    [Serializable]
    [XmlRoot("TD")]
    public class TestData
    {
        [XmlElement(ElementName = "CQD")]
        public int CurrentQuestionDifficulty { get; set; }
        
        [XmlElement(ElementName = "CTI")]
        public int CurrentTestId { get; set; }

        [XmlElement(ElementName = "CTS")]
        public int CurrentTestSeed { get; set; }

        [XmlElement(ElementName = "IT")]
        public int ItemsTaken { get; set; }

        [XmlElement(ElementName = "TDU")]
        public int TotalDifficultiesUsed { get; set; }

        [XmlElement(ElementName = "RAC")]
        public int RightAnswersCount { get; set; }

        [XmlElement(ElementName = "S")]
        public DateTime Started { get; set; }

        [XmlElement(ElementName = "RG")]
        public string ResultGraph { get; set; }

        /// <summary>
        /// Возвращает хэш от вопроса по ключу и сиду
        /// </summary>
        /// <returns></returns>
        public string GetQuestionHash()
        {
            return MD5HashManager.GenerateKey(CurrentTestId + CurrentTestSeed);
        }

        /// <summary>
        /// Генерирует следующий вопрос и кладет его в поле CurrentQuestion
        /// </summary>
        /// <returns></returns>
        public GeneratedTest NextQuestion()
        {
            throw new NotImplementedException();
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

        public void AddPointToResultGraph()
        {

        }
        
    }

    public class Answer
    {
        public int ID { get; set; }
        public string Body { get; set; }
        public bool IsCorrect { get; set; }

        [ForeignKey("Question")]
        public int QuestionID { get; set; }

        public virtual Question Question { get; set; }
    }

    /// <summary>
    /// Уже сгенерированый по шаблону тест
    /// </summary>
    [NotMapped]
    [XmlRoot("GT")]
    public class GeneratedTest
    {
        [XmlElement(ElementName = "B")]
        public string Body { get; set; }

        [XmlArray(ElementName = "AS")]
        [XmlArrayItem("A")]
        public List<GeneratedAnswer> Answers { get; set; }

        [XmlElement(ElementName = "T")]
        public int Type { get; set; }
    }

    
    [NotMapped]
    [XmlRoot("GA")]
    public class GeneratedAnswer
    {
        [XmlElement(ElementName = "B")]
        public string Body { get; set; }

        [XmlElement(ElementName = "IC")]
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