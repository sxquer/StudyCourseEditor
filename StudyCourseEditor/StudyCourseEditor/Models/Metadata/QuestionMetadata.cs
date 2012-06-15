using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudyCourseEditor.Models.Metadata
{
    public class QuestionMetadata
    {
        /// <summary>
        /// Question's template
        /// </summary>
        [DisplayName("Содержание")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Body { get; set; }

        [DisplayName("Сложность")]
        [Range(1, 10)]
        public int Difficulty { get; set; }

        [DisplayName("Опубликован")]
        [DefaultValue(true)]
        public bool IsPublished { get; set; }
    }
}