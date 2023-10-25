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
    public class PeopleService : EntityService<Person>, IPeopleService
    {
        public PeopleService(EntityFaker<Person> faker) : base(faker)
        {
        }
    }
}
