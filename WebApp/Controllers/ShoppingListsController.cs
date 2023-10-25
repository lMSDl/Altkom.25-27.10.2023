using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApp.Controllers
{
    public class ShoppingListsController : ApiController
    {

        ICollection<ShoppingList> _shoppingLists = new List<ShoppingList>();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Yield();
            return Ok(_shoppingLists);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            await Task.Yield();
            var shoppingList = _shoppingLists.SingleOrDefault(x => x.Id == id);

            if (shoppingList is null)
                return NotFound();

            return Ok(shoppingList);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, ShoppingList shoppingList)
        {
            await Task.Yield();
            var localShoppingList = _shoppingLists.SingleOrDefault(x => x.Id == id);

            if (localShoppingList is null)
                return NotFound();

            _shoppingLists.Remove(localShoppingList);
            _shoppingLists.Add(shoppingList);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(ShoppingList shoppingList)
        {
            await Task.Yield();

            shoppingList.Id = _shoppingLists.Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;
            _shoppingLists.Add(shoppingList);

            return CreatedAtAction(nameof(Get), new { id = shoppingList.Id }, shoppingList);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Task.Yield();
            var shoppingList = _shoppingLists.SingleOrDefault(x => x.Id == id);

            if (shoppingList is null)
                return NotFound();

            _shoppingLists.Remove(shoppingList);

            return NoContent();
        }
    }
}
