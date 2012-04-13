using System.ComponentModel.DataAnnotations;
using StudyCourseEditor.Models.Metadata;

namespace StudyCourseEditor.Models
{
	[MetadataType(typeof(TagMetadata))]
	public partial class Tag
	{
	}
}