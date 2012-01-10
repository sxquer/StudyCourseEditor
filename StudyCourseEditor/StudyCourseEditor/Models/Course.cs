using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}