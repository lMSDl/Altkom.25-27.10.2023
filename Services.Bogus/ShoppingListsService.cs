using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;

namespace Services.Bogus
{
    public class ShoppingListsService : IShoppingListsService
    {
        private ICollection<ShoppingList> Entities { get; }

        /*public ShoppingListsService()
        {
            Entities = new List<ShoppingList>();
        }*/
        public ShoppingListsService(ShoppingListFaker faker)
        {
            Entities = faker.Generate(15);
        }

        public Task<ShoppingList> CreateAsync(ShoppingList shoppingList)
        {
            shoppingList.Id = Entities.Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;
            Entities.Add(shoppingList);
            return Task.FromResult(shoppingList);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await ReadAsync(id);
            if(entity is not null)
                Entities.Remove(entity);
        }

        public Task<IEnumerable<ShoppingList>> ReadAsync()
        {
            return Task.FromResult(Entities.ToList().AsEnumerable());
        }

        public Task<ShoppingList?> ReadAsync(int id)
        {
            return Task.FromResult( Entities.SingleOrDefault(x => x.Id == id) );
        }

        public async Task UpdateAsync(int id, ShoppingList shoppingList)
        {
            await DeleteAsync(id);
            shoppingList.Id = id;
            Entities.Add(shoppingList);
        }
    }
}