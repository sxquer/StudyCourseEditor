using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Course
    {
    
        public int ID { get; set; }

        [Required]
        [MinLength(3)]
        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }
        
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}