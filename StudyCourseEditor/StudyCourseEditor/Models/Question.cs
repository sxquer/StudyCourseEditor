using System.ComponentModel.DataAnnotations;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Question from database
    /// </summary>
    [MetadataType(typeof (QuestionMetadata))]
    public partial class Question
    {
    }
}