using Bogus.DataSets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var parent = new Parent() { Name = "Ala" };

            var child = new Child{Name = "Adam"};
            child.Parent = parent;
            parent.Children = new List<Child> { child};

            return Ok(parent);
        }

    }


    public class Parent
    {
        public string Name { get; set; }
        public IEnumerable<Child> Children { get;set; }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }
    }
}
