using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakers
{
    public class ProductFaker : EntityFaker<Product>
    {
        public ProductFaker()
        {
            RuleFor(x => x.Name, x => x.Commerce.Product());
            //RuleFor(x => x.Price, x => float.Parse(x.Commerce.Price()));
            RuleFor(x => x.ShoppingListId, x => new Random(x.Random.Number(1000)).Next(16, 30));
        }
    }
}
