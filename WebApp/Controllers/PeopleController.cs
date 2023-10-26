using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using WebApp.Filters;

namespace WebApp.Controllers
{
    [ServiceFilter(typeof(ConsoleLogFilter))]
    public class PeopleController : EntityController<Person>
    {
        public PeopleController(IPeopleService service) : base(service)
        {
        }

        [ServiceFilter(typeof(UniquePersonFilter))]
        public override Task<IActionResult> Post(Person entity)
        {
            return base.Post(entity);
        }
    }
}
