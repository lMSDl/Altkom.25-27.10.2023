using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;

namespace WebApp.Filters
{
    public class ConsoleLogFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("After " + context.HttpContext.Request.Method);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Before " + context.HttpContext.Request.Method);
        }
    }
}
