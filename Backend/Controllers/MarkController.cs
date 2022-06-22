using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Backend.Models;
using System.Net;
using mARkIt.Backend.Utils;
using mARkIt.Backend.DataObjects;

namespace Backend.Controllers
{
    public class MarkController : TableController<Mark>
    {
        MobileServiceContext context;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<Mark>(context, Request);
        }

        public string LoggedUserId => this.GetLoggedUserId();

        // GET tables/Mark
        public IQueryable<Mark> GetAllMark()
        {
            return Query();
        }

        // GET tables/Mark/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Mark> GetMark(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Mark/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        public Task<Mark> PatchMark(string id, Delta<Mark> patch)
        {
            validateOwner(id);
            return UpdateAsync(id, patch);
        }

        // POST tables/Mark
        [Authorize]
        public async Task<IHttpActionResult> PostMark(Mark item)
        {
            item.UserId = LoggedUserId;
            Mark current = await InsertAsync(item);

            // Add a UserMarkExperience between the mark and it's creator to avoid notifying the user about it
            await context.InsertUserMarkExperience(LoggedUserId, current.Id);

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Mark/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Authorize]
        public Task DeleteMark(string id)
        {
            validateOwner(id);
            return DeleteAsync(id);
        }

        private void validateOwner(string id)
        {
            var result = Lookup(id).Queryable.Where(item => item.UserId.Equals(LoggedUserId)).FirstOrDefault();
            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }
    }
}
