using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")] //adres naszego kontrolera - w nawiasach kwadratowych nazwa klasy bez "Controller"
    [ApiController] // oznaczamy nasz kontroler jako API
    public class ValuesController : ControllerBase
    {
        public ValuesController(List<int> values)
        {
            _values = values;
        }

        List<int> _values;


        [HttpGet]
        public IEnumerable<int> Get()
        {
            return _values;
        }

        [HttpPost("{value:int:max(50)}")]
        public void Post(int value)
        {
            _values.Add(value);
        }

        [HttpPut("{oldValue:int}/{newValue:int}")]
        public void Put(int oldValue, int newValue)
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
