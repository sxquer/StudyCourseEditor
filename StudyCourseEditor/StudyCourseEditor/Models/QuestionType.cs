using System.ComponentModel.DataAnnotations;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Question type from database
    /// </summary>
    [MetadataType(typeof (QuestionTypeMetadata))]
    public partial class QuestionType
    {
    }
}