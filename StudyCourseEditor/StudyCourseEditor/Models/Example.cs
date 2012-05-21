using System.ComponentModel.DataAnnotations;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
    /// <summary>
    /// Examples for Definitions
    /// </summary>
    [MetadataType(typeof (ExampleMetadata))]
    public partial class Example
    {
    }
}