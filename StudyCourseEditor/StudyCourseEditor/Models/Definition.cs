using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Definition
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }

        public virtual ICollection<Example> Examples { get; set; }
        public virtual ICollection<DefTagRelation> DefTagRelations { get; set; }
    }
}