using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IProductsService : IEntityService<Product>
    {
        Task<IEnumerable<Product>> FirdByShoppingListId(int shoppingListId);
    }
}
