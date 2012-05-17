﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Range(1, 10)]
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