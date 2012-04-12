using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Definition
    {
        public int ID { get; set; }
        /// <summary>
        /// Caption for definition
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Text of definition
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// List of examples and antiexamples for definition
        /// </summary>
        public virtual ICollection<Example> Examples { get; set; }

        /// <summary>
        /// Link to Tags associated with this definition
        /// </summary>
        public virtual ICollection<DefTagRelation> DefTagRelations { get; set; }
    }
}