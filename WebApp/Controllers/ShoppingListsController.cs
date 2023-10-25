using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace WebApp.Controllers
{
    public class ShoppingListsController : ApiController
    {

        private IShoppingListsService _service;

        public ShoppingListsController(IShoppingListsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.ReadAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var shoppingList = await _service.ReadAsync(id);

            if (shoppingList is null)
                return NotFound();

            return Ok(shoppingList);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, ShoppingList shoppingList)
        {
            var localShoppingList = await _service.ReadAsync(id);

            if (localShoppingList is null)
                return NotFound();

            await _service.UpdateAsync(id, shoppingList);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(ShoppingList shoppingList)
        {
            shoppingList = await _service.CreateAsync(shoppingList);

            return CreatedAtAction(nameof(Get), new { id = shoppingList.Id }, shoppingList);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var shoppingList = await _service.ReadAsync(id);
            if (shoppingList is null)
                return NotFound();

           await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
