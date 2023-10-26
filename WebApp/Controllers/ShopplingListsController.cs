using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using WebApp.Filters;

namespace WebApp.Controllers
{
    public class ShoppingListsController : EntityController<ShoppingList>
    {
        private IShoppingListsService _service;
        private IProductsService _childController;

        public ShoppingListsController(IShoppingListsService service, IProductsService childController) : base(service)
        {
            _service = service;
            _childController = childController;
        }

        [HttpGet("{parentId}/Products")]
        [Produces("application/xml")] //wymuszenie zwracania xml

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetForList(int parentId)
        {
            if (_service.ReadAsync(parentId) is null)
                return NotFound();

            return Ok(await _childController.FirdByShoppingListId(parentId));
        }

        [ServiceFilter(typeof(ConsoleLogFilter))]
        public override Task<IActionResult> Post(ShoppingList entity)
        {
            return base.Post(entity);
        }
    }
}
