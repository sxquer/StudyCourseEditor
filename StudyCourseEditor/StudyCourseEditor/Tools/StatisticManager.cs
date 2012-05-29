using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyCourseEditor.Controllers;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Tools
{
    public static class StatisticManager
    {
        /// <summary>
        /// Calculate best difficulty level based on pass/fail attempts
        /// </summary>
        /// <param name="question">Question</param>
        public static int GetQuestionBestDifficultyLevel(Question question)
        {
            var result = (question.TotalAttempts == 0) ? 5 : 10 - (int)(Math.Round((float)question.RightAttempts * 10 / question.TotalAttempts));
            return (result == 0) ? 1 : result;
        }
    }
}