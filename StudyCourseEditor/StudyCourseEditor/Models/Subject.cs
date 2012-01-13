using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Subject
    {
        public int ID { get; set; }
        
        [Required]
        [MinLength(3)]
        [DisplayName("Название")]
        public string Name { get; set; }


        [DisplayName("Содержание")]
        public string Body { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<Test> Tests { get; set; } 
    }
}