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
    public class ResultController : Controller
    {
        private Entities db = new Entities();

        public static void Add(Result result)
        {
            var db = new Entities();
            db.Results.AddObject(result);
            db.SaveChanges();
        }

    }
}