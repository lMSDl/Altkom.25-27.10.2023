using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Services.Interfaces;
using WebApp.Filters;
using WebApp.SignalR;

namespace WebApp.Controllers
{
    [ServiceFilter(typeof(ConsoleLogFilter))]
    [Authorize]
    public class PeopleController : EntityController<Person>
    {
        private IHubContext<PeopleHub> _peopleHub;

        public PeopleController(IPeopleService service, IHubContext<PeopleHub> peopleHub) : base(service)
        {
            _peopleHub = peopleHub;
        }

        [Authorize(Roles = "Read")]
        public override Task<IEnumerable<Person>> Get()
        {
            return base.Get();
        }

        [ServiceFilter(typeof(UniquePersonFilter))]
        [Authorize(Roles = "Create")]
        public override Task<IActionResult> Post(Person entity)
        {
            return base.Post(entity);
        }

        public override async Task<IActionResult> Delete(int id)
        {
            var result = await base.Delete(id);

            await _peopleHub.Clients.All.SendAsync("PersonDeleted", id);

            return result;
        }
    }
}
