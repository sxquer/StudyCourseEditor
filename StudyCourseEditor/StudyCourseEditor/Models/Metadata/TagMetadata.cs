using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudyCourseEditor.Models.Metadata
{
    public class TagMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Название")]
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [StringLength(256,
            ErrorMessage = "Поле 'Название' не может превышать 256 символов")]
        public string Name { get; set; }
    }
}