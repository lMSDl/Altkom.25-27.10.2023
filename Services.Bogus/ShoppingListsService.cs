﻿using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class ShoppingListsService : EntityService<ShoppingList>, IShoppingListsService
    {
        public ShoppingListsService(EntityFaker<ShoppingList> faker) : base(faker)
        {
        }
    }
}
