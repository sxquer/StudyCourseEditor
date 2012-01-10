using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class Example
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public bool IsAntiExample { get; set; }

        [ForeignKey("Definition")]
        public int DefinitionID { get; set; }
        public virtual Definition Definition { get; set; }
    }
}