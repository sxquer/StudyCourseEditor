using System;
using System.Collections.Generic;
using System.Linq;
using StudyCourseEditor.Models;
using StudyCourseEditor.TestClasses;
using Answer = StudyCourseEditor.Models.Answer;
using Question = StudyCourseEditor.Models.Question;

namespace StudyCourseEditor.Tools
{
    public class TemplateManager
    {
        private const string TagStartInsert = "<?", TagEndInsert = "?>";
        private static readonly Entities Db = new Entities();

        /// <summary>
        /// Returns GeneratedQuestion 
        /// </summary>
        /// <param name = "templateId">Template id</param>
        /// <param name = "seed">Random seed</param>
        /// <returns></returns>
        public static GeneratedQuestion Generate(int templateId, int seed)
        {
            var result = new GeneratedQuestion();
            var rnd = new Random(seed);

            // Get template from _db
            Question question =
                Db.Questions.FirstOrDefault(q => q.ID == templateId);
            if (question == null) throw new Exception("Template not found");

            result.Body = question.Body;
            result.QuestionType = 1;
            result.Answers = new List<GeneratedAnswer>();

            // Insert definitions to Body
            int tagsLength = TagStartInsert.Length + TagEndInsert.Length;
            for (int i = 0; i < result.Body.Length - tagsLength; i++)
            {
                if (result.Body.Substring(i, TagStartInsert.Length) !=
                    TagStartInsert) continue;

                // We found start insert tag
                int tagStartInsertPosition = i;
                int definitionNamePosition = i + TagStartInsert.Length;
                for (i = definitionNamePosition;
                     i < result.Body.Length - TagEndInsert.Length;
                     i++)
                {
                    if (result.Body.Substring(i, TagEndInsert.Length) !=
                        TagEndInsert) continue;

                    // We found end insert tag
                    string definitionName =
                        result.Body.Substring(definitionNamePosition,
                                              i - definitionNamePosition);
                    if (!string.IsNullOrEmpty(definitionName))
                    {
                        // Get definition from _db
                        Definition definition =
                            Db.Definitions.FirstOrDefault(
                                d => d.Name == definitionName);
                        if (definition != null)
                        {
                            // Get examples to definition from _db
                            List<Example> examples =
                                definition.Examples.Where(e => !e.IsAntiExample)
                                    .ToList();
                            if (examples.Count == 0)
                                throw new Exception("Definition has no examples");

                            Example example =
                                examples[rnd.Next(0, examples.Count)];

                            // Replacing
                            string oldSubstr = TagStartInsert + definitionName +
                                               TagEndInsert;
                            result.Body = result.Body.Replace(oldSubstr,
                                                              example.Value.Trim
                                                                  ());
                            i = tagStartInsertPosition + example.Value.Length -
                                1;
                        }
                    }

                    break;
                }
            }

            // Get answers from _db
            List<Answer> answers = question.Answers.ToList();
            if (answers.Count == 0)
                throw new Exception("Question has no answers");
            foreach (Answer answer in answers)
            {
                var newAnswer = new GeneratedAnswer
                                    {
                                        Body = answer.Body,
                                        IsCorrect = answer.IsCorrect
                                    };

                result.Answers.Add(newAnswer);
            }

            result.QuestionType = question.QuestionTypeID;
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