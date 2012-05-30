using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudyCourseEditor.Models.Metadata
{
    public class DefinitionMetadata
    {
        /// <summary>
        /// Unique Caption for definition
        /// </summary>
        [DisplayName("Полное название")]
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [MinLength(3,
            ErrorMessage =
                "Поле 'Название' не может содержать меньше 3 символов")]
        [StringLength(256,
            ErrorMessage = "Поле 'Название' не может превышать 256 символов")]
       /* [Remote("CheckNameIsUnique", "Definition",
            ErrorMessage =
                "Полное имя должно быть уникальным. Похожее определение уже есть"
            )]*/
        public string Name { get; set; }

        /// <summary>
        /// Caption for definition displayed in text
        /// </summary>
        [DisplayName("Отображаемое название")]
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [MinLength(3,
            ErrorMessage =
                "Поле 'Название' не может содержать меньше 3 символов")]
        [StringLength(256,
            ErrorMessage = "Поле 'Название' не может превышать 256 символов")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Text of definition
        /// </summary>
        [DisplayName("")]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Body { get; set; }
    }
}