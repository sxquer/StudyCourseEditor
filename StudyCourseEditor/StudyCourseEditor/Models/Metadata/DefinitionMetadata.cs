using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudyCourseEditor.Models.Metadata
{
	public class DefinitionMetadata
	{
		/// <summary>
		/// Caption for definition
		/// </summary>
		[DisplayName("Название")]
		[Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
		[MinLength(3, ErrorMessage = "Поле 'Название' не может содержать меньше 3 символов")]
		[StringLength(256, ErrorMessage = "Поле 'Название' не может превышать 256 символов")]
		public string Name { get; set; }

		/// <summary>
		/// Text of definition
		/// </summary>
		[DisplayName("")]
		public string Body { get; set; }
	}
}