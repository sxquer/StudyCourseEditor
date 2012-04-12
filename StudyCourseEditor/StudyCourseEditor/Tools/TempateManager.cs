using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyCourseEditor.Interfaces;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Tools
{
    public class TempateManager: IQuestionGenerator
    {
        public GeneratedQuestion Generate(int templateId, int seed)
        {
            throw new NotImplementedException();
        }

        public GeneratedQuestion Generate(int templateId, out int seed)
        {
            throw new NotImplementedException();
        }

        public int GetQuestionID(int courseId, int difficulty)
        {
            throw new NotImplementedException();
        }

        public int GetRandomSeed()
        {
            throw new NotImplementedException();
        }

    }
}