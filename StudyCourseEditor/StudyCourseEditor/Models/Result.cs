using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Question type from database
    /// </summary>
    [MetadataType(typeof(ResultMetadata))]
    public partial class Result
    {
    }
}