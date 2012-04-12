using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Course contains few subjects, which have list of question
    /// </summary>
    public class Subject
    {
        public int ID { get; set; }
        
        /// <summary>
        /// Name of the subject
        /// </summary>
        [Required]
        [MinLength(3)]
        [DisplayName("Название")]
        public string Name { get; set; }

        /// <summary>
        /// Subject's main content
        /// </summary>
        [DisplayName("Содержание")]
        public string Body { get; set; }

        [ForeignKey("Course")]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        /// <summary>
        /// Questions list associated with this subject
        /// </summary>
        public virtual ICollection<Question> Questions { get; set; } 
    }
}