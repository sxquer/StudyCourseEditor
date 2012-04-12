using System.Data.Entity;

namespace StudyCourseEditor.Models
{
    public class StudyCourseDB : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DefTagRelation> DefTagRelations { get; set; }
    }
}