using Backend.Models;
using mARkIt.Backend.DataObjects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace mARkIt.Backend.Utils
{
    public static class Extensions
    {
        public static string GetLoggedUserId(this ApiController apiController)
        {
            string userId = null;

            var claim = ((ClaimsPrincipal)apiController.User).FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                userId = claim.Value;

                // Azure places "sid:" at the start of the generated id, which we want to remove to avoid redundancy
                // the to avoid sending the ':' character in HTTP uri's since it causes an error.
                userId = userId.Replace("sid:", "");
            }

            return userId;
        }

        public async static Task<bool> InsertUserMarkExperience(this MobileServiceContext context, string userId, string markId)
        {
            bool updateWasSuccessful = false;

            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    UserMarkExperience userMarkExperience = context.UserMarkExperiences.Create();
                    userMarkExperience.UserId = userId;
                    userMarkExperience.MarkId = markId;
                    userMarkExperience.HasUserRated = false;
                    userMarkExperience.LastSeen = DateTime.Now;
                    context.UserMarkExperiences.Add(userMarkExperience);

                    await context.SaveChangesAsync();
                    transaction.Commit();
                    updateWasSuccessful = true;
                }

                catch (Exception ex)
                {
                    LogTools.LogException(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }

            return updateWasSuccessful;
        }

        public static int GetUserRelevantCateogiresCode(this MobileServiceContext context, string userId)
        {
            User user = context.Users.Find(userId);
            return user.RelevantCategoriesCode;
        }
    }
}