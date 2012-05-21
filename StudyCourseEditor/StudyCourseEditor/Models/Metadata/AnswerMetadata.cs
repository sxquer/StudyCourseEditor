using System.ComponentModel;

namespace StudyCourseEditor.Models.Metadata
{
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