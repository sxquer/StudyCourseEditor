using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.Models
{
    public class StudyCourseDB : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Test> Tests { get; set; }
    }
}