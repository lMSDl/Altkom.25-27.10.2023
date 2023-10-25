using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakers
{
    public class ShoppingListFaker : EntityFaker<ShoppingList>
    {
        public ShoppingListFaker()
        {
            RuleFor(x => x.Name, x => x.Commerce.Categories(1)[0]);
        }
    }
}
