using Models;
using Services.Interfaces;

namespace WebApp.Controllers
{
    public class PeopleController : EntityController<Person>
    {
        public PeopleController(IPeopleService service) : base(service)
        {
        }
    }
}
