using Backend.Models;
using Microsoft.Azure.Mobile.Server.Config;
using System;
using System.Web.Http;
using mARkIt.Backend.Utils;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity;
using mARkIt.Backend.DataObjects;

namespace mARkIt.Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class SeenMarkController : ApiController
    {
        MobileServiceContext context;
        private string LoggedUserId => this.GetLoggedUserId();

        public SeenMarkController()
        {
            context = new MobileServiceContext();
        }

        // POST api/SeenMark
        [HttpPost]
        public async Task<bool> Post(string markId)
        {
            bool updateWasSuccessful = false;

            if (markId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            UserMarkExperience userMarkExperience = await context.UserMarkExperiences.FindAsync(LoggedUserId, markId);
            if (userMarkExperience == null)
            {
                updateWasSuccessful = await context.InsertUserMarkExperience(LoggedUserId, markId);
            }
            else
            {
                updateWasSuccessful = await updateLastSeenTime(markId);
            }

            return updateWasSuccessful;
        }

        private async Task<bool> updateLastSeenTime(string markId)
        {
            bool updateWasSuccessful = false;

            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    UserMarkExperience userMarkExperience = await context.UserMarkExperiences.FindAsync(LoggedUserId, markId);
                    validateOwner(userMarkExperience);

                    userMarkExperience.LastSeen = DateTime.Now;

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

        private void validateOwner(UserMarkExperience userMarkRating)
        {
            if (userMarkRating.UserId != LoggedUserId)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }
    }
}