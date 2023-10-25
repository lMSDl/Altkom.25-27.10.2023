using Models;

namespace Services.Interfaces
{
    public interface IShoppingListsService
    {
        Task<ShoppingList> CreateAsync(ShoppingList shoppingList);
        Task<IEnumerable<ShoppingList>> ReadAsync();
        Task<ShoppingList?> ReadAsync(int id);
        Task UpdateAsync(int id, ShoppingList shoppingList);
        Task DeleteAsync(int id);
    }
}