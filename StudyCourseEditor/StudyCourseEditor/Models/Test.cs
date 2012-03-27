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
        public int ItemsTaken { get; set; }
        public int TotalDifficultiesUsed { get; set; }

    }
}