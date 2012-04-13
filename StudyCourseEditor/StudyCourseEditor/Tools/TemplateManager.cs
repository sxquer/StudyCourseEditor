using System;
using System.Collections.Generic;
using System.Linq;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Tools
{
    public class TemplateManager
    {
        private static readonly Entities Db = new Entities();

        private const string TagStartInsert = "<?", TagEndInsert = "?>";

        /// <summary>
        /// 	Возвращает сгенерированный вопрос по заданному шаблону и сиду
        /// </summary>
        /// <param name = "templateId">ID шаблона</param>
        /// <param name = "seed">Сид</param>
        /// <returns>Сгенерированный вопрос</returns>
        public static GeneratedQuestion Generate(int templateId, int seed)
        {
            var result = new GeneratedQuestion();
            var rnd = new Random(seed);

            // Get template from _db
            var question = Db.Questions.FirstOrDefault(q => q.ID == templateId);
            if (question == null) throw new Exception("Template not found");

            result.Body = question.Body;
            result.Type = 1;
            result.Answers = new List<GeneratedAnswer>();

            // Insert definitions to Body
            var tagsLength = TagStartInsert.Length + TagEndInsert.Length;
            for (var i = 0; i < result.Body.Length - tagsLength; i++)
            {
                if (result.Body.Substring(i, TagStartInsert.Length) != TagStartInsert) continue;

                // We found start insert tag
                var tagStartInsertPosition = i;
                var definitionNamePosition = i + TagStartInsert.Length;
                for (i = definitionNamePosition; i < result.Body.Length - TagEndInsert.Length; i++)
                {
                    if (result.Body.Substring(i, TagEndInsert.Length) != TagEndInsert) continue;

                    // We found end insert tag
                    var definitionName = result.Body.Substring(definitionNamePosition, i - definitionNamePosition);
                    if (!string.IsNullOrEmpty(definitionName))
                    {
                        // Get definition from _db
                        var definition = Db.Definitions.FirstOrDefault(d => d.Name == definitionName);
                        if (definition != null)
                        {
                            // Get examples to definition from _db
                            var examples = definition.Examples.Where(e => !e.IsAntiExample).ToList();
                            if (examples.Count == 0) throw new Exception("Definition has no examples");

                            var example = examples[rnd.Next(0, examples.Count)];

                            // Replacing
                            var oldSubstr = TagStartInsert + definitionName + TagEndInsert;
                            result.Body = result.Body.Replace(oldSubstr, example.Value.Trim());
                            i = tagStartInsertPosition + example.Value.Length - 1;
                        }
                    }

                    break;
                }
            }

            // Get answers from _db
            var answers = question.Answers.ToList();
            if (answers.Count == 0) throw new Exception("Question has no answers");
            foreach (var answer in answers)
            {
                var newAnswer = new GeneratedAnswer
                {
                    Body = answer.Body,
                    IsCorrect = answer.IsCorrect
                };

                result.Answers.Add(newAnswer);
            }

            return result;
        }

        /// <summary>
        /// 	Returns randomly generated seed
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            return new Random().Next();
        }
    }
}