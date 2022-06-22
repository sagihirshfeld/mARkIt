using System.Web.Http;
using System;
using System.Security.Claims;
using System.Data.Entity.Validation;

namespace mARkIt.Backend.Utils
{
    public static class LogTools
    {
        public static void LogDbEntityValidationException(DbEntityValidationException ex)
        {
            Log("Logging the details of DbEntityValidationException:");

            foreach (var eve in ex.EntityValidationErrors)
            {
                Log($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");

                foreach (var ve in eve.ValidationErrors)
                {
                    Log($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                }
            }
        }

        public static void LogException(Exception ex)
        {
            Log($"{ex.GetBaseException().GetType().Name}: {ex.GetBaseException().Message}");
        }

        public static void Log(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }
    }
}