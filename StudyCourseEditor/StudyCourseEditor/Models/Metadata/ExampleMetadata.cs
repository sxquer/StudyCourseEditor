using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudyCourseEditor.Models.Metadata
{
	public class ExampleMetadata
	{
		/// <summary>
		/// Example text
		/// </summary>
		[DisplayName("Значение")]
		[Required(ErrorMessage = "Поле 'Значение' обязательно для заполнения")]
		[StringLength(256, ErrorMessage = "Поле 'Значение' не может превышать 256 символов")]
		public string Value { get; set; }

		/// <summary>
		/// Checked when example is ANTIexample
		/// </summary>\
		[DisplayName("Анти-пример")]
		public bool IsAntiExample { get; set; }
	}
}