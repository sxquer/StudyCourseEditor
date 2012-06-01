using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using StudyCourseEditor.Models;

namespace StudyCourseEditor.Tools
{
    /// <summary>
    /// Handles test session in database
    /// </summary>
    public static class TestSessionManager
    {
        private static Entities db = new Entities();
        
        /// <summary>
        /// Save session
        /// </summary>
        /// <param name="data">Test data</param>
        /// <param name="sessionId">Session id</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets session from database
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <returns></returns>
        public static string Read(int sessionId)
        {
            var session = db.TestSessions.FirstOrDefault(x => x.ID == sessionId);
            return (session != null) ? session.ObjectData : null;
        }

        /// <summary>
        /// Remove session from database
        /// </summary>
        /// <param name="sessionId">Session id</param>
        public static void Remove(int sessionId)
        {
            var session = db.TestSessions.FirstOrDefault(x => x.ID == sessionId);
            db.TestSessions.DeleteObject(session);
            db.SaveChanges();
        }

    }
}