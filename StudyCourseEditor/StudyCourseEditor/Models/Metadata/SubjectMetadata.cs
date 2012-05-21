using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudyCourseEditor.Models.Metadata
{
    public class SubjectMetadata
    {
        /// <summary>
        /// Name of the subject
        /// </summary>
        [DisplayName("Название")]
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [MinLength(3,
            ErrorMessage =
                "Поле 'Название' не может содержать меньше 3 символов")]
        [StringLength(256,
            ErrorMessage = "Поле 'Название' не может превышать 256 символов")]
        public string Name { get; set; }

        /// <summary>
        /// Subject's main content
        /// </summary>
        [DisplayName("Содержание")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Body { get; set; }
    }
}