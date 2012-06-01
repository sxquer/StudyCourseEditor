using System;

namespace StudyCourseEditor.Tools
{
    /// <summary>
    /// Handles time
    /// </summary>
    public class TimeManager
    {
        /// <summary>
        /// Server can be located in another time zone. Tune this method to fix this difference.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}