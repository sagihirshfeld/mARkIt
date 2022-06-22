using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using System.Windows;
using mARkIt.Backend.Utils;
using mARkIt.Backend.DataObjects;

namespace Backend.Controllers
{
    [MobileAppController]
    [Authorize]
    public class RelevantMarksController : ApiController
    {
        private const double k_DefaultProximityThreshhold = 10;

        MobileServiceContext context;

        private string LoggedUserId => this.GetLoggedUserId();

        public RelevantMarksController()
        {
            context = new MobileServiceContext();
        }

        // GET api/RelevantMarks
        public List<Mark> Get(double? longitude, double? latitude, double? proximityThreshhold)
        {
            List<Mark> relevantMarksByCategoryAndProximity = null;

            int relevantCategoriesCode = context.GetUserRelevantCateogiresCode(LoggedUserId);

            var relevantMarksByCategory = from mark in context.Marks
                                          where (relevantCategoriesCode & mark.CategoriesCode) != 0
                                          select mark;

            if (longitude.HasValue && latitude.HasValue)
            {
                if (!proximityThreshhold.HasValue)
                {
                    proximityThreshhold = k_DefaultProximityThreshhold;
                }

                relevantMarksByCategoryAndProximity = new List<Mark>();
                foreach (Mark mark in relevantMarksByCategory)
                {
                    Vector userPos = new Vector(longitude.Value, latitude.Value);
                    Vector markPos = new Vector(mark.Longitude, mark.Latitude);
                    if (markIsCloseEnough(userPos, markPos, proximityThreshhold.Value))
                    {
                        relevantMarksByCategoryAndProximity.Add(mark);
                    }
                }
            }
            else
            {
                relevantMarksByCategoryAndProximity = relevantMarksByCategory.ToList();
            }

            return relevantMarksByCategoryAndProximity;
        }

        private bool markIsCloseEnough(Vector userPos, Vector markPos, double proximityThreshhold)
        {
            return DistanceCalculator.DistanceInKm(userPos, markPos) < proximityThreshhold;
        }
    }
}