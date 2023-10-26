using Microsoft.AspNetCore.Mvc;
using Models;
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
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.ReadAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _service.ReadAsync(id);

            if (entity is null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPut("{id:int}")]
        public virtual async Task<IActionResult> Put(int id, T entity)
        {
            var localEntity = await _service.ReadAsync(id);

            if (localEntity is null)
                return NotFound();

            await _service.UpdateAsync(id, entity);

            return NoContent();
        }

        [HttpPost]
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
