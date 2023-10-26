using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using Services.Interfaces;

namespace WebApp.Controllers
{
    public abstract class EntityController<T> : ApiController where T : Entity
    {

        private IEntityService<T> _service;

        public EntityController(IEntityService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<T>> Get()
        {
            return (await _service.ReadAsync()).AsEnumerable();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _service.ReadAsync(id);

            if (entity is null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Put(int id, T entity)
        {
            var localEntity = await _service.ReadAsync(id);

            if (localEntity is null)
                return NotFound();

            await _service.UpdateAsync(id, entity);

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual async Task<IActionResult> Post(T entity)
        {
            //Ręczna walidacja modelu
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            entity = await _service.CreateAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _service.ReadAsync(id);
            if (entity is null)
                return NotFound();

           await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
