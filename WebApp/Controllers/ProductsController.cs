using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApp.Controllers
{
    public class ProductsController : EntityController<Product>
    {
        private IProductsService _service;
        private IShoppingListsService _parentService;

        public ProductsController(IProductsService service, IShoppingListsService parentService) : base(service)
        {
            _service = service;
            _parentService = parentService;
        }

        /*[HttpGet($"/api/{nameof(ShoppingList)}s/{{parentId}}/Products")]
        public async Task<IActionResult> GetForList(int parentId)
        {
            if (_parentService.ReadAsync(parentId) is null)
                return NotFound();

            return Ok(await _service.FirdByShoppingListId(parentId));
        }*/


        [NonAction] // wyłączenie ankcji z obsługi - usługa zachowuje się tak, jakby ta metoda nie była w ogóle zaimplementowana (kod 405)
        public override Task<IActionResult> Put(int id, Product entity)
        {
            return base.Put(id, entity);
        }
    }
}
