using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Net;
using System;
using Backend.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using mARkIt.Backend.Utils;
using mARkIt.Backend.DataObjects;

namespace Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class RatingController : ApiController
    {
        MobileServiceContext context;
        private string LoggedUserId => this.GetLoggedUserId();

        public RatingController()
        {
            context = new MobileServiceContext();
        }

        // GET api/Rating
        [HttpGet]
        public double? Get(string markId)
        {
            // Check parameters validity
            if (markId == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            UserMarkExperience userMarkExperience = context.UserMarkExperiences.Find(LoggedUserId, markId);

            if (userMarkExperience == null)
            {
                return null;
            }
            else
            {
                return userMarkExperience.UserRating;
            }
        }

        // POST api/Rating
        [HttpPost]
        public async Task<bool> Post(string markId, float? rating)
        {
            bool updateWasSuccessful = false;

            // Check parameters
            if (markId == null || rating == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            UserMarkExperience userMarkExperience = await context.UserMarkExperiences.FindAsync(LoggedUserId, markId);
            if (userMarkExperience == null)
            {
                await context.InsertUserMarkExperience(LoggedUserId, markId);
            }

            updateWasSuccessful = await updateMarkRating(markId, rating);

            return updateWasSuccessful;
        }

        private async Task<bool> updateMarkRating(string markId, float? rating)
        {
            bool updateWasSuccessful = false;

            using (DbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    UserMarkExperience userMarkExperience = await context.UserMarkExperiences.FindAsync(LoggedUserId, markId);
                    validateOwner(userMarkExperience);

                    userMarkExperience.Mark.RatingsSum -= userMarkExperience.UserRating;
                    userMarkExperience.Mark.RatingsSum += rating.Value;
                    userMarkExperience.UserRating = rating.Value;
                    userMarkExperience.LastSeen = DateTime.Now;

                    if (!userMarkExperience.HasUserRated)
                    {
                        userMarkExperience.Mark.RatingsCount++;
                        userMarkExperience.HasUserRated = true;
                    }

                    userMarkExperience.Mark.UpdateRating();
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