using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

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
        public async Task<IActionResult> GetForList(int parentId)
        {
            if (_service.ReadAsync(parentId) is null)
                return NotFound();

            return Ok(await _childController.FirdByShoppingListId(parentId));
        }
    }
}
