using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace StudyCourseEditor.TestClasses
{
    [XmlRoot("QB")]
    public class QuestionBank
    {
        public QuestionBank()
        {
            Questions = new List<List<int>>();
        }
        
        [XmlArrayItem("DF")]
        public List<List<int>> Questions { get; set; }

        /// <summary>
        /// Total amount of questions in bank
        /// </summary>
        public int QuestionCount { 
            get 
            { 
                return FlatQuestionList.Count;
            }   
        }

        /// <summary>
        /// Linear representation of question bank
        /// </summary>
        public List<int> FlatQuestionList {
            get
            {
                var result = new List<int>();
                for (int i = 1; i <= Questions.Count; i++) result.AddRange(GetQuestionList(i));

                return result;
            }
        }

        /// <summary>
        /// Returns random question id with specified 'difficulty' or -1
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public int GetQuestion(int difficulty)
        {
            var candidates = GetQuestionList(difficulty);
            if (candidates.Count < 1) return -1;
            return candidates[new Random().Next(candidates.Count)];
        }

        /// <summary>
        /// Returns random question
        /// </summary>
        /// <returns></returns>
        public int GetRandomQuestion()
        {
            var candidates = FlatQuestionList;
            if (candidates.Count == 0) return -1;

            return candidates[new Random().Next(candidates.Count - 1)];
        }


        /// <summary>
        /// Removes question from bank
        /// </summary>
        /// <param name="id">Question id</param>
        /// <param name="difficulty">Difficulty level. If 0, Qustion searching in entire bank</param>
        public void RemoveQuestion(int id, int difficulty = 0)
        {
            if (difficulty > 0) 
            {
                GetQuestionList(difficulty).Remove(id);
            }
            else
            {
                for (int i = 1; i <= Questions.Count; i++)
                    GetQuestionList(i).Remove(id);
            }

        }

        /// <summary>
        /// Gets And Removes question from bank with specidied difficulty
        /// </summary>
        /// <param name="difficulty">Necessary difficulty level</param>
        /// <returns></returns>
        public int GetAndRemove(int difficulty)
        {
            int result = GetQuestion(difficulty);
            if (result == -1) return result;
            RemoveQuestion(result, difficulty);
            return result;
        }

        /// <summary>
        /// Gets And Removes question from bank
        /// </summary>
        /// <returns></returns>
        public int RandomGetAndRemove()
        {
            int result = GetRandomQuestion();
            if (result == -1) return result;
            RemoveQuestion(result);
            return result;
        }

        /// <summary>
        /// Returns question list with specified difficulty level
        /// </summary>
        /// <param name="difficulty">Difficulty level</param>
        /// <returns></returns>
        public List<int> GetQuestionList(int difficulty)
        {
            return Questions[difficulty - 1];
        }

    }


    public class EmptyDifficultyException : Exception
    {
        public EmptyDifficultyException(int difficulty)
            : base(String.Format("Вопросы сложности '{0}' не найдены", difficulty))
            {
            }
    }
}