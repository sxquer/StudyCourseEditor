using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyCourseEditor.Tools
{
    public class TimeManager
    {
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
