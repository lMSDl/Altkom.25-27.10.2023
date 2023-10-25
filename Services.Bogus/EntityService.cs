using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;

namespace Services.Bogus
{
    public class EntityService<T> : IEntityService<T> where T : Entity
    {
        protected ICollection<T> Entities { get; }

        /*public EntityService()
        {
            Entities = new List<T>();
        }*/
        public EntityService(EntityFaker<T> faker)
        {
            Entities = faker.Generate(15);
        }

        public Task<T> CreateAsync(T entity)
        {
            entity.Id = Entities.Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;
            Entities.Add(entity);
            return Task.FromResult(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await ReadAsync(id);
            if(entity is not null)
                Entities.Remove(entity);
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(Entities.ToList().AsEnumerable());
        }

        public Task<T?> ReadAsync(int id)
        {
            return Task.FromResult( Entities.SingleOrDefault(x => x.Id == id) );
        }

        public async Task UpdateAsync(int id, T entity)
        {
            await DeleteAsync(id);
            entity.Id = id;
            Entities.Add(entity);
        }
    }
}