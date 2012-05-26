using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyCourseEditor.TestClasses
{
    /// <summary>
    /// Type of sourses for tests
    /// </summary>
    public class TestSourceTypes
    {
        /// <summary>
        /// Manually choosed subjects
        /// </summary>
        public const int COMBINATION = 0;

        /// <summary>
        /// All subjects from selected course
        /// </summary>
        public const int COURSE = 1;

        /// <summary>
        /// One subject
        /// </summary>
        public const int SUBJECT = 2;

    }
}