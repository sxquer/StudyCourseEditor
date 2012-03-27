using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class DefTagRelation
    {
        public int ID { get; set; }
        
        [ForeignKey("Tag")]
        public int TagID{ get; set; }
        public virtual Tag Tag { get; set; }

        [ForeignKey("Definition")]
        public int DefinitionID { get; set; }
        public virtual Definition Definition { get; set; }
    }
}