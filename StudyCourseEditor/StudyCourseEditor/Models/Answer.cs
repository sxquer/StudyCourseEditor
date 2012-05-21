using System.ComponentModel.DataAnnotations;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Answer from database
    /// </summary>
    [MetadataType(typeof (AnswerMetadata))]
    public partial class Answer
    {
    }
}