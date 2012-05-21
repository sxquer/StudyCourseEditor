using System.ComponentModel.DataAnnotations;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Course contains few subjects, which have list of question
    /// </summary>
    [MetadataType(typeof (SubjectMetadata))]
    public partial class Subject
    {
    }
}