using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Tools
{
    public static class TestSessionManager
    {
        private static Entities db = new Entities();
        public static int Write(string data, int sessionId)
        {
            
            var session = db.TestSessions.FirstOrDefault(x => x.ID == sessionId);
            if (session != null)
            {
                session.ObjectData = data;
                db.ObjectStateManager.ChangeObjectState(session, EntityState.Modified);
                db.SaveChanges();
                return session.ID;
            }
            else
            {
                session = new TestSession
                              {
                                  ObjectData = data,
                                  Date = TimeManager.GetCurrentTime(),
                              };
                db.TestSessions.AddObject(session);
                db.SaveChanges();
                return session.ID;
            }
        }

        public static string Read(int sessionId)
        {
            var session = db.TestSessions.FirstOrDefault(x => x.ID == sessionId);
            return (session != null) ? session.ObjectData : null;
        }

        public static void Remove(int sessionId)
        {
            var session = db.TestSessions.FirstOrDefault(x => x.ID == sessionId);
            db.TestSessions.DeleteObject(session);
            db.SaveChanges();
        }

    }
}