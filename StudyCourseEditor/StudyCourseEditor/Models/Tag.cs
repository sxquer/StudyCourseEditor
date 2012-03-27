using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Tag
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<DefTagRelation> DefTagRelations { get; set; }

    }
}