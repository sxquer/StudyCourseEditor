using System.ComponentModel.DataAnnotations;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
	[MetadataType(typeof(CourseMetadata))]
	public partial class Course
	{
	}
}