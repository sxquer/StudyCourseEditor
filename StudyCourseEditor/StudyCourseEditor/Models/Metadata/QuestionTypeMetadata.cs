using System.ComponentModel;

namespace StudyCourseEditor.Models.Metadata
{
    public class QuestionTypeMetadata
    {
        [DisplayName("Имя типа")]
        public string Name { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }
    }
}