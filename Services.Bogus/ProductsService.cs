using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class ProductsService : EntityService<Product>, IProductsService
    {
        public ProductsService(EntityFaker<Product> faker) : base(faker)
        {
        }

        public Task<IEnumerable<Product>> FirdByShoppingListId(int shoppingListId)
        {
            return Task.FromResult(Entities.Where(x => x.ShoppingListId == shoppingListId).ToList().AsEnumerable());
        }
    }
}
