using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Services.Interfaces;

namespace WebApp.Filters
{
    public class UniquePersonFilter : IAsyncActionFilter
    {

        private IPeopleService _service;

        public UniquePersonFilter(IPeopleService service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var entity = context.ActionArguments["entity"] as Person;
            var duplicate = (await _service.ReadAsync()).Where(x => x.FirstName == entity.FirstName).Any(x => x.LastName == entity.LastName);
            if(duplicate)
            {
                context.ModelState.AddModelError(nameof(Models.Person), "First and Last name must be unique");
            }

            if(!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            await next();

        }
    }
}
