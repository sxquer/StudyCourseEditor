using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Controllers
{ 
    /// <summary>
    /// This controller will handle results
    /// </summary>
    public class ResultController : Controller
    {
        /// <summary>
        /// Add test result to database
        /// </summary>
        /// <param name="result">Result instance</param>
        public static void Add(Result result)
        {
            var db = new Entities();
            db.Results.AddObject(result);
            db.SaveChanges();
        }

    }
}