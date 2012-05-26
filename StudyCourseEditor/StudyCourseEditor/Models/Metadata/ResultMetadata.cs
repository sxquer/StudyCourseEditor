using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models.Metadata
{
    public class ResultMetadata
    {
        [Required]
        public float Measure { get; set; }
        [Required]
        public string ResultGraph { get; set; }
        
        /// <summary>
        /// Says what type of sourse was used to generate test
        /// If 'Combination' - Link should be empty
        /// See 'TestSourceType' class
        /// </summary>
        [Required]
        public int SourceType { get; set; }
        public int? Source { get; set; }
    }


}