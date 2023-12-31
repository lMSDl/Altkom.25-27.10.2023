﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Filters;

namespace WebApp.Controllers
{
    //adnotacje dziedziczone z ApiController
    //[Route("api/[controller]")]
    //[ApiController] 
    [ServiceFilter(typeof(LimiterFilter))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public class ValuesController : ApiController
    {
        public ValuesController(List<int> values)
        {
            _values = values;
        }

        List<int> _values;

        //GET: localhost:<port>/api/values
        [HttpGet] //adnotacja określająca typ/czasownik zapytania
        public IEnumerable<int> Get(CancellationToken cancellationToken)
        {
            Task.Delay(5000, cancellationToken).Wait();

            return _values;
        }

        //Post: localhost:<port>/api/values/<value>
        [HttpPost("{value:int:max(50)}")] //routing doklejany do adresu kontrolera
        [HttpPost("alamakota/{value:int:max(50)}")] //metoda dostępna pod dodatkowym adresem /api/[controller]/alamakota/...
        public void Post(int value)
        {
            _values.Add(value);
        }

        [HttpPut("/alamakota/{oldValue:int}/{newValue:int}")] // ukośnik "/" na początku routingu powoduje, że nie doklejamy do adresu kontrolera, ale tworzymy nowy adres od roota
        public void Put(int oldValue, int newValue)
        {
            _values[_values.IndexOf(oldValue)] = newValue;
        }

        //Put: localhost:<port>/api/values/<oldValue>?newValue=<someValue>
        [HttpPut("{oldValue:int}")] 
        public void Put2(int oldValue, int newValue)
        {
            _values[_values.IndexOf(oldValue)] = newValue;
        }

        [HttpDelete("{value:int}")]
        public void Delete(int value)
        {
            _values.Remove(value);
        }
    }
}
