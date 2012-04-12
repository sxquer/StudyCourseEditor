﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyCourseEditor.Interfaces;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Tools
{
    public class TemplateManager
    {
        /// <summary>
        /// Возвращает сгенерированный вопрос по заданному шаблону и сиду
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <param name="seed">Сид</param>
        /// <returns>Сгенерированный вопрос</returns>
        public static GeneratedQuestion Generate(int templateId, int seed)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Возвращает ID шаблона из заданного курса, со сложностью, максимально близкой к difficulty
        /// </summary>
        /// <param name="courseId">ID курса</param>
        /// <param name="difficulty">Теоретическая сложность вопроса</param>
        /// <returns>ID шаблона</returns>
        public static int GetQuestionID(int courseId, int difficulty)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns randomly generated seed
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            throw new NotImplementedException();
        }

    }
}