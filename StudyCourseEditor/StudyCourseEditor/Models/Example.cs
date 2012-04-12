using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Examples for Definitions
    /// </summary>
    public class Example
    {
        public int ID { get; set; }
        
        /// <summary>
        /// Example text
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Checked when example is ANTIexample
        /// </summary>
        public bool IsAntiExample { get; set; }


        /// <summary>
        /// Link to Definition
        /// </summary>
        [ForeignKey("Definition")]
        public int DefinitionID { get; set; }
        public virtual Definition Definition { get; set; }
    }
}