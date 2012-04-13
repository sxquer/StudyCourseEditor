using System.ComponentModel;

namespace StudyCourseEditor.Models.Metadata
{
	public class QuestionMetadata
	{
		/// <summary>
		/// Question's template
		/// </summary>
		[DisplayName("Содержание")]
		public string Body { get; set; }

		[DisplayName("Сложность")]
		public int Difficulty { get; set; }
	}

	public class AnswerMetadata
	{
		/// <summary>
		/// Answer Template
		/// </summary>
		[DisplayName("")]
		public string Body { get; set; }

		/// <summary>
		/// Checked if answer is correct
		/// </summary>
		[DisplayName("")]
		public bool IsCorrect { get; set; }
	}
}