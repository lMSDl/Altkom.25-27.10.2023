using Models;
using Services.Interfaces;

namespace WebApp.Controllers
{
    public class ShoppingListsController : EntityController<ShoppingList>
    {
        public ShoppingListsController(IShoppingListsService service) : base(service)
        {
        }
    }
}
