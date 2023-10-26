using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using WebApp.Filters;

namespace WebApp.Controllers
{
    [ServiceFilter(typeof(ConsoleLogFilter))]
    [Authorize]
    public class PeopleController : EntityController<Person>
    {
        public PeopleController(IPeopleService service) : base(service)
        {
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
    }
}
