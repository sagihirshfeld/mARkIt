using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using mARkIt.Abstractions;
using mARkIt.Services;

namespace mARkIt.Models
{
    public class Mark : TableData
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public string Style { get; set; }
        public int CategoriesCode { get; set; }
        public int RatingsCount { get; set; }
        public float Rating { get; set; }

        public static async Task<List<Mark>> GetMyMarks()
        {
            List<Mark> marks;

            try
            {
                marks = await AzureWebApi.MobileService.GetTable<Mark>().Where(mark => mark.UserId == App.ConnectedUser.Id).ToListAsync();
            }
            catch
            {
                marks = null;
            }

            return marks;
        }

        public async static Task<Mark> GetById(string i_Id)
        {
            Mark mark = await AzureWebApi.GetById<Mark>(i_Id);
            return mark;
        }

        /// <summary>
        /// Get a list of marks filtered by categories and/or location.
        /// </summary>
        /// <param name="i_Longitube"> Can be omitted if filteration by location is not desired - only if i_Latitude is ommited as well </param>
        /// <param name="i_Latitude"> Can be omitted if filteration by location is not desired - only if i_Longitude is ommited as well </param>
        /// <returns>
        /// A list of marks if the fetching operation was successful, otherwise returns null;        
        /// </returns>
        public static async Task<List<Mark>> GetRelevantMarks(double? i_Longitube = null, double? i_Latitude = null, double? i_ProximityThreshhold = null)
        {
            List<Mark> relevantMarks;

            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "longitude", i_Longitube.ToString() },
                { "latitude", i_Latitude.ToString()},
                {"proximityThreshhold",i_ProximityThreshhold.ToString()}
            };

            try
            {
                relevantMarks = await AzureWebApi.MobileService.InvokeApiAsync<List<Mark>>("RelevantMarks", HttpMethod.Get, parameters);
            }
            catch(Exception)
            {
                relevantMarks = null;
            }

            return relevantMarks;
        }

        public static async Task<bool> Insert(Mark i_Mark)
        {
            return await AzureWebApi.Insert(i_Mark);
        }

        public static async Task<bool> Delete(Mark i_Mark)
        {
            return await AzureWebApi.Delete(i_Mark);
        }

        public static async Task<bool> Update(Mark i_Mark)
        {
            return await AzureWebApi.Update(i_Mark);
        }
    }
}
