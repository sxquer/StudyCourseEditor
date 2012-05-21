using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [XmlElement(ElementName = "CQD")]
        public int CurrentQuestionDifficulty { get; set; }

        [XmlElement(ElementName = "TDL")]
        public double TrueDifficultyLevel { get; set; }

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
        /// 	Data for graphic presentation of test's results. Format "difficulty_isCorrect;"
        /// </summary>
        [XmlElement(ElementName = "RG")]
        public string ResultGraph { get; set; }

        /// <summary>
        /// 	Возвращает хэш от вопроса по ключу и сиду
        /// </summary>
        /// <returns></returns>
        public string GetQuestionHash()
        {
            return MD5HashManager.GenerateKey(CurrentQuestionId + TestSeed);
        }


        /// <summary>
        /// 	Возвращает текущую ошибку измерения. Необходима для критерия остановки.
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

            return
                Math.Sqrt(ItemsTaken / (wrongAnswersCount * rightAnswersCount));
        }

        /// <summary>
        /// 	Возвращает примерную оценку уровня знаний студента.
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
    }

    


}