using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Test
    {
        public int ID { get; set; }
        public string Body { get; set; }
        public int Difficulty { get; set; }
    }

    [NotMapped]
    public class TestData
    {
        public int CurrentQuestionDifficulty { get; set; }
        public GeneratedTest CurrentTest { get; set; }
        public int ItemsTaken { get; set; }
        public int TotalDifficultiesUsed { get; set; }
        public int RightAnswersCount { get; set; }
        public DateTime Started { get; set; }
        public string ResultGraph { get; set; }

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

    /// <summary>
    /// Уже сгенерированый по шаблону тест
    /// </summary>
    [NotMapped]
    public class GeneratedTest
    {
        public string Body { get; set; }
        public List<Answer> Answers { get; set; }
        public QuestionType Type { get; set; }
    }

    [NotMapped]
    public class Answer
    {
        public string Body { get; set; }
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