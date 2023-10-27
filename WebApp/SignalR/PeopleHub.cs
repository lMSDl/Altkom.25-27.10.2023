using Microsoft.AspNetCore.SignalR;
using Models;
using Services.Interfaces;

namespace WebApp.SignalR
{
    public class PeopleHub : Hub
    {
        private IPeopleService _service;

        public PeopleHub(IPeopleService service)
        {
            _service = service;
        }

        public async Task AddPerson(Person person)
        {
            await _service.CreateAsync(person);
        }
    }
}
