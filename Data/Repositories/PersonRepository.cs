using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class PersonRepository : AbstractRepository, IPersonRepository
    {
        private readonly DbSet<Person> _people;

        public PersonRepository(MathQuestDbContext context)
            : base(context)
        {
            this._people = context.Set<Person>();
        }

        public async Task AddAsync(Person entity)
        {
            await this._people.AddAsync(entity);
        }

        public void Delete(Person entity)
        {
            this._people.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _people.FindAsync(id);

            if (entity != null)
            {
                this._people.Remove(entity);
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await this._people.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await this._people.FindAsync(id) ?? new Person();
        }

        public void Update(Person entity)
        {
            if (entity != null)
            {
                var existingEntity = this._people.Find(entity.Id);

                if (existingEntity != null)
                {
                    this._context.Entry(existingEntity).State = EntityState.Detached;
                    this._people.Update(entity);
                }
            }
        }
    }
}
