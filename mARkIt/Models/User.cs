using mARkIt.Abstractions;
using mARkIt.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using mARkIt.Utils;
using System.Net.Http;

namespace mARkIt.Models
{
    public class User : TableData
    {
        public int relevantCategoriesCode { get; set; }

        public static async Task<bool> Insert(User i_User)
        {
            return await AzureWebApi.Insert(i_User);
        }

        public static async Task<bool> Delete(User i_User)
        {
            return await AzureWebApi.Delete(i_User);
        }

        public static async Task<bool> Update(User i_User)
        {
            return await AzureWebApi.Update(i_User);
        }

        public static async Task<bool> UpdateMarkSeen(string i_MarkId)
        {
            bool updateSuccessful = false;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "markId",i_MarkId }
            };

            try
            {
                updateSuccessful = await AzureWebApi.MobileService.InvokeApiAsync<bool>("SeenMark", HttpMethod.Post, parameters);
            }
            catch (Exception)
            {
                updateSuccessful = false;
            }

            return updateSuccessful;
        }

        /// <summary>
        /// Posts a new rating of mark by a user, or update his rating for the same mark.
        /// </summary>
        /// <param name="i_MarkId"></param>
        /// <param name="i_Rating"></param>
        /// <returns>
        /// Returns 'true' if the rating was added/updated successfully, 'false' otherwise.
        /// </returns>
        /// <remarks>
        /// Also returns 'false' if the user or the mark do not exist in the database.
        /// </remarks>
        public static async Task<bool> RateMark(string i_MarkId, float i_Rating)
        {
            bool updateSuccessful = false;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "markId",i_MarkId },
                { "rating", i_Rating.ToString()}
            };

            try
            {
                updateSuccessful = await AzureWebApi.MobileService.InvokeApiAsync<bool>("Rating", HttpMethod.Post, parameters);
            }
            catch(Exception)
            {
                updateSuccessful = false;
            }

            return updateSuccessful;
        }

        /// <summary>
        /// Get the rating an existing user gave to an existing mark.
        /// </summary>
        /// <param name="i_MarkId"></param>
        /// <returns>
        /// Returns a 'double?' which will hold the rating if it was fetched successfully, 'null' otherwise. 
        /// </returns>
        /// <remarks>
        /// Also returns 'null' if a rating of the mark by the user does not exist in the database.
        /// </remarks>
        public static async Task<float?> GetUserRatingForMark(string i_MarkId)
        {
            float? userRatingOfMark;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "markId",i_MarkId }
            };

            try
            {
                userRatingOfMark = await AzureWebApi.MobileService.InvokeApiAsync<float?>("Rating", HttpMethod.Get, parameters);
            }
            catch(Exception)
            {
                return userRatingOfMark = null;
            }

            return userRatingOfMark;
        }
    }
}
