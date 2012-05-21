using StudyCourseEditor.Models;
using StudyCourseEditor.TestClasses;

namespace StudyCourseEditor.Interfaces
{
    public interface IQuestionGenerator
    {
        /// <summary>
        /// Возвращает сгенерированный вопрос по заданному шаблону и сиду
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <param name="seed">Сид</param>
        /// <returns>Сгенерированный вопрос</returns>
        GeneratedQuestion Generate(int templateId, int seed);

        /// <summary>
        /// Возвращает ID шаблона из заданного курса, со сложностью, максимально близкой к difficulty
        /// </summary>
        /// <param name="courseId">ID курса</param>
        /// <param name="difficulty">Теоретическая сложность вопроса</param>
        /// <returns>ID шаблона</returns>
        int GetQuestionID(int courseId, int difficulty);

        /// <summary>
        /// Returns randomly generated seed
        /// </summary>
        /// <returns></returns>
        int GetRandomSeed();
    }
}